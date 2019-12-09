using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace muroLast
{  /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
    public partial class MainWindow : Window
    {
        int roomId = 0;
        int itemId = 0;
        int binaId = 0;
        int il, say;
        List<Room> rooms;
        List<Item> items;
        List<Bina> binas;

        public MainWindow()
        {

            InitializeComponent();
            updateData();
        }

        private void updateData()
        {
            using (var context = new UniContext())
            {
                // Load all students and related enrollments
                rooms = context.Rooms.ToList();
                items = context.Items.ToList();
                binas = context.Binas.ToList();
                otaqsave.ItemsSource = rooms;

                otaqsave.DisplayMemberPath = "Name";
                otaqsave.SelectedValuePath = "RoomID";
                otaqsave.SelectedIndex = 0;

                inventarsave.ItemsSource = items;
                inventarsave.DisplayMemberPath = "Name";
                inventarsave.SelectedValuePath = "ItemID";
                inventarsave.SelectedIndex = 0;

                binasave.ItemsSource = binas;
                binasave.DisplayMemberPath = "Name";
                binasave.SelectedValuePath = "BinaId";
                binasave.SelectedIndex = 0;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(saysave.Text) |
                 String.IsNullOrEmpty(mssave.Text) |
                   rooms==null|
                   items==null|
                   binas==null|
                 string.IsNullOrEmpty(otaqsave.SelectedValue.ToString()) | 
                 string.IsNullOrEmpty(inventarsave.SelectedValue.ToString())|
                 string.IsNullOrEmpty(binasave.SelectedValue.ToString()) 
               )
            {
                ersave.Content = "Bütün xanaları Doldurun";
                return;
            }
            using (var context = new UniContext())
            {
               
                    roomId = (int) otaqsave.SelectedValue;
                    itemId = (int) inventarsave.SelectedValue;
                    binaId = (int) binasave.SelectedValue;
                


                if (!int.TryParse(saysave.Text, out say))
                    say = 1;

                if (!int.TryParse(ilsave.Text, out il))
                    il = 0;


                var r = context.Rooms.Find(roomId);
                var b = context.Binas.Find(binaId);
                var inventar = new Inventar
                {

                    SN = snsave.Text,
                    Count = say,
                    Year = il,
                    Person = mssave.Text,
                    Note = new TextRange(qeydsave.Document.ContentStart, qeydsave.Document.ContentEnd).Text,
                    CreateDate = DateTime.Now,
                    Room = r,
                    Bina=b,
                    Item = context.Items.Find(itemId),
                };
                var inven = context.Inventars.Add(inventar);
                try
                {

                    context.SaveChanges();
                    string barkod = ("SB" + r.Name + "R" + b.Name + "F" + "-"+inven.InventarID).Replace(" ", String.Empty);
                    inven.Barcode = barkod;
                    context.SaveChanges();
                    updateGridData();
                    snsave.Clear();
                    qeydsave.Document.Blocks.Clear();
                    mssave.Clear();
                    saysave.Clear();
                    ilsave.Clear();
                    ersave.Content = "Yaddaşa verildi";

                    string dataDir = System.AppDomain.CurrentDomain.BaseDirectory;


                    ersave.Content = "Şəkil düzəlir..";
                    BarcodeLib.Barcode ba = new BarcodeLib.Barcode();
                    System.Drawing.Image img = ba.Encode(BarcodeLib.TYPE.CODE128, barkod, System.Drawing.Color.Black, System.Drawing.Color.White,450,180);
                    if (!Directory.Exists("C:/barkodlar/"))
                    {
                        Directory.CreateDirectory("C:/barkodlar/");
                    }
                    if (!Directory.Exists("C:/barkodlar/"+r.Name))
                    {
                        Directory.CreateDirectory("C:/barkodlar/"+r.Name);
                    }
                    if (!Directory.Exists("C:/barkodlar/" + r.Name+"/"+b.Name))
                    {
                        Directory.CreateDirectory("C:/barkodlar/" + r.Name + "/" + b.Name);
                    }
                    for (int i = 0; i < say; i++)
                    {
                        img.Save("C:/" + "/barkodlar/" + r.Name + "/" + b.Name + "/" + barkod+"c"+i + ".png", ImageFormat.Png);
                    }
                 
                    barimage.Source = new BitmapImage(new Uri("C:/" + "/barkodlar/" + r.Name + "/" + b.Name + "/" + barkod+"c0" + ".png"));
                    ersave.Content = "yadda saxlanıldı";
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    ersave.Content = ex;
                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (item == null)
            {
                return;
            }
            view.Visibility = Visibility.Hidden;
            edit.Visibility = Visibility.Visible;
            snedit.Text = item.SN;
            qeydedit.Document.Blocks.Add(new Paragraph(new Run(item.Note)));
            sayedit.Text = item.Count.ToString();
            iledit.Text = item.Year.ToString();
            msedit.Text = item.Person;


            otaqedit.ItemsSource = rooms;
            otaqedit.DisplayMemberPath = "Name";
            otaqedit.SelectedValuePath = "RoomID";
            otaqedit.SelectedValue = item.Room.RoomID;

            adedit.ItemsSource = items;
            adedit.DisplayMemberPath = "Name";
            adedit.SelectedValuePath = "ItemID";
            adedit.SelectedValue = item.Item.ItemID;

            binaedit.ItemsSource = binas;
            binaedit.DisplayMemberPath = "Name";
            binaedit.SelectedValuePath = "BinaId";
            binaedit.SelectedValue = item.Bina.BinaId;


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            view.Visibility = Visibility.Visible;
            edit.Visibility = Visibility.Hidden;

            if (otaqedit.SelectedValue != null)
            {
                roomId = int.Parse(otaqedit.SelectedValue.ToString());
            }
            if (adedit.SelectedValue != null)
            {
                itemId = int.Parse(adedit.SelectedValue.ToString());
            }
            if (binaedit.SelectedValue != null)
            {
                binaId = int.Parse(binaedit.SelectedValue.ToString());
            }

            using (var context = new UniContext())
            {

                item = context.Inventars.Where(i => i.Barcode == searchtxt.Text).FirstOrDefault();
                item.SN = snedit.Text;
                item.Note = new TextRange(qeydedit.Document.ContentStart, qeydedit.Document.ContentEnd).Text;
                item.Count = int.Parse(sayedit.Text);
                item.Year = int.Parse(iledit.Text);
                item.Person = msedit.Text;
                item.Room = context.Rooms.Find(roomId);
                item.Item = context.Items.Find(itemId);
                item.Bina = context.Binas.Find(binaId);
                
                context.SaveChanges();
            }
            ShowView();
        }

        private Inventar item;

        private void searchtxt_changed(object sender, TextChangedEventArgs e)
        {
            using (var context = new UniContext())
            {
                item = context.Inventars.Where(i => i.Barcode == searchtxt.Text).FirstOrDefault();
                if (item != null)
                {
                    ShowView();
                    var inventar = new Checked
                    {
                        ChechkedTime = DateTime.Now,
                        Inventar=item,
                       
                  };
                    context.CheckedItem.Add(inventar);
                    context.SaveChanges();
                }

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            updateGridData();
            updateViewData();
            // TODO: Add code here to load data into the table cehcedItem.
            // This code could not be generated, because the uniContextDataSet1cehcedItemTableAdapter.Fill method is missing, or has unrecognized parameters.

            // TODO: Add code here to load data into the table checkedItem.
            // This code could not be generated, because the uniContextDataSet1checkedItemTableAdapter.Fill method is missing, or has unrecognized parameters.
            //muroLast.UniContextDataSetTableAdapters.checkedItemTableAdapter uniContextDataSet1checkedItemTableAdapter = new muroLast.UniContextDataSetTableAdapters.checkedItemTableAdapter();
            //System.Windows.Data.CollectionViewSource checkedItemViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("checkedItemViewSource")));
            //checkedItemViewSource.View.MoveCurrentToFirst();
        }

        private void updateGridData()
        {
            muroLast.UniContextDataSet uniContextDataSet = ((muroLast.UniContextDataSet)(this.FindResource("UniContextDataSet")));
            // Load data into the table Inventars. You can modify this code as needed.
            muroLast.UniContextDataSetTableAdapters.NewSelectCommandTableAdapter
                uniContextDataSetInventarsTableAdapter = new muroLast.UniContextDataSetTableAdapters.NewSelectCommandTableAdapter();
            uniContextDataSetInventarsTableAdapter.Fill(uniContextDataSet.NewSelectCommand);
            System.Windows.Data.CollectionViewSource inventarsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventarsViewSource")));
            inventarsViewSource.View.MoveCurrentToFirst();
        }

        private void updateViewData()
        {
            muroLast.UniContextDataSet uniContextDataSet = ((muroLast.UniContextDataSet)(this.FindResource("UniContextDataSet")));
            // Load data into the table Inventars. You can modify this code as needed.
            muroLast.UniContextDataSetTableAdapters.checkedItemTableAdapter
            uniContextDataSetInventarsTableAdapter = new muroLast.UniContextDataSetTableAdapters.checkedItemTableAdapter();
            uniContextDataSetInventarsTableAdapter.Fill(uniContextDataSet.checkedItem);
            System.Windows.Data.CollectionViewSource inventarsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("checkedItemViewSource")));
            inventarsViewSource.View.MoveCurrentToFirst();
        }
        private void ShowView()
        {
            snview.Content = item.SN;
            qeydview.Content = item.Note;
            sayiew.Content = item.Count;
            ilview.Content = item.Year;
            msview.Content = item.Person;
            otaqview.Content = item.Room.Name;
            adview.Content = item.Item.Name;
            binaview.Content = item.Bina.Name;

        }

        private void copyDataGridContentToClipboard()
        {

            if (tabcontrol.TabIndex==2)
            {
                watchedDataGridView.SelectAll();
                watchedDataGridView.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;

                ApplicationCommands.Copy.Execute(null, watchedDataGridView);
                watchedDataGridView.UnselectAll();
            }
            else
            {
                dataGridView.SelectAll();
                dataGridView.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;

                ApplicationCommands.Copy.Execute(null, dataGridView);
                dataGridView.UnselectAll();
            }
          
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            using (var context = new UniContext())
            {
                if (!string.IsNullOrEmpty(adAdd.Text))
                {
                    var item = new Item
                    {
                        Name = adAdd.Text,
                    };
                    context.Items.Add(item);
                }
                if (!string.IsNullOrEmpty(inventarAdd.Text))
                {
                    var room = new Room
                    {
                        Name = inventarAdd.Text,
                    };
                    context.Rooms.Add(room);
                }
                if (!string.IsNullOrEmpty(binaAdd.Text))
                {
                    var bina = new Bina
                    {
                        Name = binaAdd.Text,
                    };
                    context.Binas.Add(bina);
                }
                context.SaveChanges();
                adAdd.Clear();
                inventarAdd.Clear();
                binaAdd.Clear();
                updateData();
            }

        }

        private void ElaveEtClicked(object sender, MouseButtonEventArgs e)
        {

        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            copyDataGridContentToClipboard();
            Microsoft.Office.Interop.Excel.Application excelApp;
            Microsoft.Office.Interop.Excel.Workbook excelWkbk;
            Microsoft.Office.Interop.Excel.Worksheet excelWksht;
            object misValue = System.Reflection.Missing.Value;
            excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = true;
            excelWkbk = excelApp.Workbooks.Add(misValue);
            excelWksht = (Microsoft.Office.Interop.Excel.Worksheet)excelWkbk.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)excelWksht.Cells[1, 1];
            CR.Select();
            excelWksht.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }

        



    }
}
