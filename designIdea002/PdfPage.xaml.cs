using designIdea002.Common;
using designIdea002.Data;
using designIdea002.Common;
using designIdea002.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234

namespace designIdea002
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for
    /// the currently selected item.
    /// </summary>
    public sealed partial class PdfPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        List<string> files_for_Delete = new List<string>();
        bool t = false;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public PdfPage()
        {
            this.InitializeComponent();

            // Setup the navigation helper
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            // Setup the logical page navigation components that allow
            // the page to only show one pane at a time.
            this.navigationHelper.GoBackCommand = new RelayCommand(() => this.GoBack(), () => this.CanGoBack());
            this.itemListView.SelectionChanged += ItemListView_SelectionChanged;

            // Start listening for Window size changes 
            // to change from showing two panes to showing a single pane
            Window.Current.SizeChanged += Window_SizeChanged;
            this.InvalidateVisualState();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            string link = e.NavigationParameter as string;
            // Aleksa: added try and catch
            try
            {
                await MainPage.downloadPdf(link);
            }
            catch (Exception exc)
            {
                MessageDialog md = new MessageDialog("Нема интернет конекције!");
                await md.ShowAsync();
                Frame.Navigate(typeof(MainPage));
            }
            await LoadPdfFileAsync();
        }
        private PdfDocument pdfDocument;

        private async System.Threading.Tasks.Task LoadPdfFileAsync()
        {
            // Aleksa TODO: test this with time
            // TimeSpan ts = DateTime.Now.TimeOfDay;
            // TimeSpan tsplus = new TimeSpan(ts.Hours, ts.Minutes, ts.Seconds + 30);

            while ((t != true) /*|| (ts.Seconds == tsplus.Seconds)*/)
            {
                // ts = DateTime.Now.TimeOfDay;
                try
                {

                    StorageFile pdfFile = await ApplicationData.Current.LocalFolder.GetFileAsync("2.pdf");
                    //Load Pdf File

                    pdfDocument = await PdfDocument.LoadFromFileAsync(pdfFile); ;


                    ObservableCollection<SampleDataItem> items = new ObservableCollection<SampleDataItem>();
                    this.DefaultViewModel["Items"] = items;

                    //StorageFolder tempFolderDel = ApplicationData.Current.TemporaryFolder;
                    // await   tempFolderDel.DeleteAsync(StorageDeleteOption.Default);

                    // Get the app's local folder.
                    //StorageFile localFile = await ApplicationData.Current.TemporaryFolder.GetFileAsync("ba4755e5-520b-42eb-bcf8-1f0cddd5e5ca.png");
                    //await localFile.DeleteAsync();

                    // Create a new subfolder in the current folder.
                    // Raise an exception if the folder already exists.
                    //string desiredName = "TempState2";
                    //StorageFolder newFolder = await localFolder.CreateFolderAsync(desiredName,CreationCollisionOption.FailIfExists);

                    if (pdfDocument != null && pdfDocument.PageCount > 0)
                    {
                        string file = "";
                        //Get Pdf page
                        for (int pageIndex = 0; pageIndex < pdfDocument.PageCount; pageIndex++)
                        {
                            var pdfPage = pdfDocument.GetPage((uint)pageIndex);
                            if (pdfPage != null)
                            {
                                // next, generate a bitmap of the page
                                StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;
                                StorageFile pngFile = await tempFolder.CreateFileAsync((file = Guid.NewGuid().ToString() + ".png"), CreationCollisionOption.ReplaceExisting);
                                files_for_Delete.Add(file);
                                if (pngFile != null)
                                {
                                    IRandomAccessStream randomStream = await pngFile.OpenAsync(FileAccessMode.ReadWrite);
                                    PdfPageRenderOptions pdfPageRenderOptions = new PdfPageRenderOptions();
                                    pdfPageRenderOptions.DestinationWidth = (uint)(this.ActualWidth - 130);

                                    await pdfPage.RenderToStreamAsync(randomStream, pdfPageRenderOptions);
                                    await randomStream.FlushAsync();
                                    randomStream.Dispose();
                                    pdfPage.Dispose();
                                    items.Add(new SampleDataItem(
                                        pageIndex.ToString(),
                                        pageIndex.ToString(),
                                        pngFile.Path));
                                }
                            }
                        }
                    }
                    t = true;

                }
                catch (Exception err)
                {
                    t = false;
                }
            }
            if (t == false)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
          
        }
        public async void deleteFilesPng(List<string> files)
        {
            for (int i = 0; i < files.Count; i++)
            {
                StorageFile sampleFile = await ApplicationData.Current.TemporaryFolder.GetFileAsync(files[i]);
                await sampleFile.DeleteAsync(StorageDeleteOption.Default);
            }
        }
        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="e">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (this.itemsViewSource.View != null)
            {
                var selectedItem = (designIdea002.Data.SampleDataItem)this.itemsViewSource.View.CurrentItem;
                if (selectedItem != null) e.PageState["SelectedItem"] = selectedItem.UniqueId;
            }
        }

        #region Logical page navigation

        // The split page isdesigned so that when the Window does have enough space to show
        // both the list and the dteails, only one pane will be shown at at time.
        //
        // This is all implemented with a single physical page that can represent two logical
        // pages.  The code below achieves this goal without making the user aware of the
        // distinction.

        private const int MinimumWidthForSupportingTwoPanes = 768;

        /// <summary>
        /// Invoked to determine whether the page should act as one logical page or two.
        /// </summary>
        /// <returns>True if the window should show act as one logical page, false
        /// otherwise.</returns>
        private bool UsingLogicalPageNavigation()
        {
            return Window.Current.Bounds.Width < MinimumWidthForSupportingTwoPanes;
        }

        /// <summary>
        /// Invoked with the Window changes size
        /// </summary>
        /// <param name="sender">The current Window</param>
        /// <param name="e">Event data that describes the new size of the Window</param>
        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        /// <summary>
        /// Invoked when an item within the list is selected.
        /// </summary>
        /// <param name="sender">The GridView displaying the selected item.</param>
        /// <param name="e">Event data that describes how the selection was changed.</param>
        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Invalidate the view state when logical page navigation is in effect, as a change
            // in selection may cause a corresponding change in the current logical page.  When
            // an item is selected this has the effect of changing from displaying the item list
            // to showing the selected item's details.  When the selection is cleared this has the
            // opposite effect.
            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();
        }

        private bool CanGoBack()
        {
            if (this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null)
            {
                return true;
            }
            else
            {
                return this.navigationHelper.CanGoBack();
            }
        }
        private void GoBack()
        {
            if (t == false)
                return;
            t = false;
            deleteFilesPng(files_for_Delete);
            if (this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null)
            {
                // When logical page navigation is in effect and there's a selected item that
                // item's details are currently displayed.  Clearing the selection will return to
                // the item list.  From the user's point of view this is a logical backward
                // navigation.
                this.itemListView.SelectedItem = null;
            }
            else
            {
                this.navigationHelper.GoBack();
            }
        }

        private void InvalidateVisualState()
        {
            var visualState = DetermineVisualState();
            VisualStateManager.GoToState(this, visualState, false);
            this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        private string DetermineVisualState()
        {
            if (!UsingLogicalPageNavigation())
                return "PrimaryView";

            // Update the back button's enabled state when the view state changes
            var logicalPageBack = this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null;

            return logicalPageBack ? "SinglePane_Detail" : "SinglePane";
        }

        #endregion

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(t==false)
            { return; }
            this.Frame.Navigate(typeof(MainPage));
            deleteFilesPng(files_for_Delete);
            t = false;
        }

        private void itemListView_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}