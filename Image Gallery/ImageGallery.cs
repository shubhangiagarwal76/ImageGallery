using C1.C1Pdf;
using C1.Win.C1Tile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_Gallery
{
    public partial class ImageGallery : Form
    {
        SplitContainer splitContainer1;
        TableLayoutPanel tableLayoutPanel1;
        Panel panel1, panel2, p;
        TextBox _searchBox;
        PictureBox _search, _exportimage;
        Tile tile1, tile2, tile3;
        Group group1;
        C1TileControl _imageTileControl;
        C1PdfDocument c1PdfDocument1;
        StatusStrip statusStrip1;
        ToolStripProgressBar toolStripProgressBar1;
        ToolStripLabel toolStripLabel, check;
        Timer timer;
        Label topic;
        AutoCompleteStringCollection data;

        PanelElement panelElement1 = new PanelElement();
        ImageElement imageElement1 = new ImageElement();
        TextElement textElement1 = new TextElement();

        DataFetcher datafetch = new DataFetcher();
        List<ImageItem> imagesList;
        int checkedItems = 0;

        public ImageGallery()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            
            this.ClientSize = new Size(790, 790);
            this.MaximizeBox = false;
            this.MaximumSize = new Size(810, 810);
            this.MinimumSize = new Size(790, 730);
            this.Name = "ImageGallery";
            this.StartPosition = FormStartPosition.CenterParent;



            this.Text = "Image Gallery";

            try
            {
                this.Icon = new Icon(@"instagram-icon.ico");
                this.ShowIcon = true;
            }
            catch
            {
                this.ShowIcon = false;
            }

            Controls.Add(addSplitContainer());
            
        }

        //ADDING SPLIT CONTAINERS(PANEL1, PANEL2)
        async SplitContainer addSplitContainer()
        {
            splitContainer1 = new SplitContainer();
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(2);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            splitContainer1.Size = new Size(631, 326);
            splitContainer1.SplitterDistance = 40;
            splitContainer1.TabIndex = 0;

            //ADDING TABLE LAYOUT TO SPLIT CONTAINER PANEL 1
            splitContainer1.Panel1.Controls.Add(addTableLayout());

            //ADDING PDF EXPORT PICTURE TO SPLIT CONTAINER PANEL 2
            splitContainer1.Panel2.Controls.Add(addExportPicture());

            //ADDIG NAMING LABEL
            splitContainer1.Panel2.Controls.Add(addName());

            //ADDING TILES TO PANEL 2 FOR IMAGE VIEWING
            splitContainer1.Panel2.Controls.Add(addTileControl());

            //ADDING STATUS 
            splitContainer1.Panel2.Controls.Add(addStatus());

            imagesList = await datafetch.GetImageData("Grapecity");

            String str = _searchBox.Text + "'s" + " Images";

            //GIBBERISH SEARCHES (EXTRA FEATURE)
            if (!imagesList.Any())
            {
                imagesList = await datafetch.GetImageData("No Image", 1);
                str = "Oopps.. No Image Found";
            }

            //CHECKING FOR NETWORK ERROR
            if (!datafetch.flag)
                str = "Network Error";

            AddTiles(imagesList);
            group1.Visible = true;

            topic.Text = str;
            topic.Visible = true;
            toolStripProgressBar1.Visible = false;



            return splitContainer1;
        }

        //CREATING TABLE LAYOUT TO ADD SPLIT CONTAINER PANEL 1
        TableLayoutPanel addTableLayout()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));

            //ADDING A PANEL TO 2ND COLUMN FOR SEARCH BOX
            tableLayoutPanel1.Controls.Add(addPanel(), 1, 0);

            //ADDING A PANEL TO 3RD COLUMN FOR SEARCH IMAGE
            tableLayoutPanel1.Controls.Add(addPanel2(), 2, 0);


            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(800, 40);
            tableLayoutPanel1.TabIndex = 0;


            return tableLayoutPanel1;
        }

        //CREATING PANEL TO ADD IN TABLE LAYOUT COLUMN 2
        Panel addPanel()
        {
            panel1 = new Panel();

            //ADDING A SEARCH BOX IN PANEL
            panel1.Controls.Add(addSearchBox());

            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(477, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(287, 40);
            panel1.TabIndex = 0;
            panel1.Paint += new PaintEventHandler(splitContainer1_Panel2_Paint);

            return panel1;
        }

        //CREATING PANEL 2 TO ADD IN TABLE LAYOUT COLUMN 3
        Panel addPanel2()
        {
            panel2 = new Panel();

            //ADDING PICTURE BOX TO PANEL
            panel2.Controls.Add(addSearchPicture());
           
            panel2.Location = new Point(479, 12);
            panel2.Margin = new Padding(2, 12, 45, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(40, 16);
            panel2.TabIndex = 1;

            return panel2;
        }


        //ADDING PICTURE FOR SEARCHING
        PictureBox addSearchPicture()
        {
            _search = new PictureBox();
            _search.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            _search.Image = global::Image_Gallery.Properties.Resources.searchpic;


            _search.Location = new Point(0, 0);
            _search.Dock = DockStyle.Left;
            _search.Margin = new Padding(0);
            _search.Name = "_search";
            _search.Size = new Size(40, 16);
            _search.SizeMode = PictureBoxSizeMode.Zoom;
            _search.TabIndex = 0;
            _search.TabStop = false;

            //CLICK EVENT ON PICTURE BOX
            _search.Click += new EventHandler(searchClick);

            return _search;
        }

        //CREATING SEARCH TEXT BOX
        TextBox addSearchBox()
        {
            _searchBox = new TextBox();
            _searchBox.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            _searchBox.BorderStyle = BorderStyle.None;
            _searchBox.Location = new Point(16, 9);
            _searchBox.Name = "_searchBox";
            _searchBox.Size = new Size(244, 16);
            _searchBox.TabIndex = 0;
            _searchBox.Text = "Search Image";

            //EXTRA FEATURE

            //CREATING STRING COLLECTION FOR AUTO COMPLETION FOR TEXT
            data = new AutoCompleteStringCollection();
            _searchBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            _searchBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            return _searchBox;
        }

        //EXTRA FEATURE OF ADDING A NAMING LABEL TO THE APPLICATION
        Panel addName()
        {
             p = new Panel();
            p.Location = new Point(300, 3);
            p.Height = 35;
            p.Width = 250;
            p.BackColor = Color.Lavender;
            p.AutoScroll = true;
            
            
            topic = new Label();
            topic.AutoSize = true;
            topic.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            topic.Location = new Point(0,0);
            topic.Name = "topic";
            topic.Size = new Size(86, 31);
            topic.TabIndex = 4;
            topic.Text = "";
            topic.Visible = false;

            p.Controls.Add(topic);

            return p;
        }


        //CREATING A PICTURE TO EXPORT PDF
        PictureBox addExportPicture()
        {
            _exportimage = new PictureBox();
            _exportimage.Image = global::Image_Gallery.Properties.Resources.index;

            _exportimage.Location = new Point(29, 3);
            _exportimage.Name = "_exportimage";
            _exportimage.Size = new Size(135, 28);
            _exportimage.SizeMode = PictureBoxSizeMode.StretchImage;
            _exportimage.TabIndex = 2;
            _exportimage.TabStop = false;
            _exportimage.Visible = false;

            //ADDING CLICK EVENT
            _exportimage.Click += new EventHandler(exportImageClick);

            //ADDING PAINT EVENT
            _exportimage.Paint += new PaintEventHandler(exportImagePaint);

            return _exportimage;
        }

       


        //ADDING TILES FOR IMAGE DISPLAY
        Tile addTile1()
        {
            tile1 = new Tile();
            tile1.BackColor = Color.LightCoral;
            tile1.Name = "tile1";
            tile1.Text = "Tile 1";
            return tile1;
        }
        Tile addTile2()
        {
            tile2 = new Tile();
            tile2.BackColor = Color.Teal;
            tile2.Name = "tile2";
            tile2.Text = "Tile 2";
            return tile2;
        }
        Tile addTile3()
        {
            tile3 = new Tile();
            tile3.BackColor = Color.SteelBlue;
            tile3.Name = "tile3";
            tile3.Text = "Tile 3";
            return tile3;
        }

        //GROUPING TILES INTO 1
        Group addGroup()
        {
            group1 = new Group();
            group1.Name = "group1";
            group1.Text = "";
            group1.Tiles.Add(addTile1());
            group1.Tiles.Add(addTile2());
            group1.Tiles.Add(addTile3());
            group1.Visible = false;
            return group1;
        }


        //CONTROLLING THE TILE GROUP
        C1TileControl addTileControl()
        {
            _imageTileControl = new C1TileControl();

            _imageTileControl.AllowChecking = true;
            _imageTileControl.AllowRearranging = true;
            _imageTileControl.BackColor = Color.Lavender;
           
            
            _imageTileControl.CellHeight = 78;
            _imageTileControl.CellSpacing = 18;
            _imageTileControl.CellWidth = 73;
           



            panelElement1.Alignment = ContentAlignment.BottomLeft;
            panelElement1.Children.Add(imageElement1);
            panelElement1.Children.Add(textElement1);
            panelElement1.Margin = new Padding(10, 6, 10, 6);


            _imageTileControl.DefaultTemplate.Elements.Add(panelElement1);
            _imageTileControl.Dock = DockStyle.Fill;
            _imageTileControl.Groups.Add(addGroup());
            _imageTileControl.Location = new Point(0, 0);
            _imageTileControl.Name = "_imageTileControl";
            _imageTileControl.Size = new Size(780, 705);
            _imageTileControl.SurfacePadding = new Padding(12, 4, 12, 4);
            _imageTileControl.SwipeDistance = 20;
            _imageTileControl.SwipeRearrangeDistance = 98;

            //ADDING CURSOR HAND WHEN HOVER ON TILE CONTROL
            _imageTileControl.Cursor = Cursors.Hand;

            //GIVING VERTICAL LAYOUT TO THE TILE CONTROL
            _imageTileControl.Orientation = LayoutOrientation.Vertical;
            _imageTileControl.TabIndex = 1;
            _imageTileControl.Text = "";

           //EVENTS
           _imageTileControl.TileChecked += new EventHandler<TileEventArgs>(imageTileControlTileChecked);
            _imageTileControl.TileUnchecked += new EventHandler<TileEventArgs>(imageTileControlTileUnchecked);
            _imageTileControl.Paint += new PaintEventHandler(imageTileControlPaint);

            return _imageTileControl;


        }

       



        //PDF CONVERSION
        C1PdfDocument addPdfDocument()
        {
            c1PdfDocument1 = new C1PdfDocument();
            c1PdfDocument1.DocumentInfo.Author = "";
            c1PdfDocument1.DocumentInfo.CreationDate = new DateTime(((long)(0)));
            c1PdfDocument1.DocumentInfo.Creator = "";
            c1PdfDocument1.DocumentInfo.Keywords = "";
            c1PdfDocument1.DocumentInfo.Producer = "ComponentOne C1Pdf";
            c1PdfDocument1.DocumentInfo.Subject = "";
            c1PdfDocument1.DocumentInfo.Title = "";
            c1PdfDocument1.MaxHeaderBookmarkLevel = 0;
            c1PdfDocument1.PdfVersion = "1.3";
            c1PdfDocument1.RefDC = null;
            c1PdfDocument1.RotateAngle = 0F;
            c1PdfDocument1.UseFastTextOut = true;
            c1PdfDocument1.UseFontShaping = true;

            return c1PdfDocument1;
        }

        //CREATING STATUS BAR
        StatusStrip addStatus()
        {
            statusStrip1 = new StatusStrip();

            //ADDING DATE, CHECKED IMAGES AND PROGRESS BAR TO STATUS BAR
            statusStrip1.Items.AddRange(new ToolStripItem[] { addDate(), addChecked(), addProgressBar(), });

            
            statusStrip1.Location = new Point(0, 683);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(780, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            statusStrip1.Visible = true;

            return statusStrip1;
        }

        //EXTRA FEATURE ADDITION OF DATE AT THE STATUS BAR
        ToolStripLabel addDate()
        {
            toolStripLabel = new ToolStripLabel();

            //ADDING TIMER
            timer = new Timer();
            timer.Tick += new EventHandler(timer1_Tick);
            toolStripLabel.Margin = new Padding(50,0,0,0);
            timer.Start();
           
           return toolStripLabel;
        }


        //EXTRA FEATURE ADDITION OF CHECKED IMAGES AT THE END
        ToolStripLabel addChecked()
        {
            check = new ToolStripLabel();

            check.AutoSize = true;
            check.Text = "Checked Images: "+ checkedItems.ToString();
            check.Margin = new Padding(200, 0, 180, 0);
            return check;
        }

        //PROGRESS BAR OPTION
        ToolStripProgressBar addProgressBar()
        {
            toolStripProgressBar1 = new ToolStripProgressBar();

            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripLabel.Margin = new Padding(0, 0, 0, 0);
            toolStripProgressBar1.Size = new Size(100, 16);
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            toolStripProgressBar1.Visible = false;

            return toolStripProgressBar1;
        }

        //UNCHECK ALL ON NEW SEARCH
        void unCheckAll()
        {
            checkedItems = 0;
            check.Text = "Checked Images: " + checkedItems.ToString();
            _exportimage.Visible = false;
        }

        
     
        


        //EVENT HANDLING STARTS FROM HERE
        
        //PAINT EVENT FOR SPLIT CONTAINER
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = _searchBox.Bounds;
            r.Inflate(3, 3);
            Pen p = new Pen(Color.LightGray);
            try
            {
                e.Graphics.DrawRectangle(p, r);
            }
            catch
            {
                Console.WriteLine("NULL VALUE RECEIVED");
            }
        }

        //CLICK EVENT ON SEARCH PICTURE
        private async void searchClick(object sender, EventArgs e)
        {
            //UNCHECK PREVIOUSLY CHECKED ITEMS
            unCheckAll();

           //ADDING STRING TO STRING COLLECTION 
            data.Add(_searchBox.Text);
            _searchBox.AutoCompleteCustomSource = data;

            toolStripProgressBar1.Visible = true;

            //FETCHING DATA
            imagesList = await datafetch.GetImageData(_searchBox.Text);

            String str = _searchBox.Text+ "'s" + " Images";

            //GIBBERISH SEARCHES (EXTRA FEATURE)
            if (!imagesList.Any())
            {
                imagesList = await datafetch.GetImageData("No Image", 1);
                str = "Oopps.. No Image Found";
            }

            //CHECKING FOR NETWORK ERROR
            if (!datafetch.flag)
                str = "Network Error";
            
            AddTiles(imagesList);
            group1.Visible = true;
            
            topic.Text = str;
            topic.Visible = true;
            toolStripProgressBar1.Visible = false;


        }

        //ADDING TILES AFTER SEARCH
        private void AddTiles(List<ImageItem> imageList)
        {
            _imageTileControl.Groups[0].Tiles.Clear();
            foreach (var imageitem in imageList)
            {
                Tile tile = new Tile();

                tile.HorizontalSize = 2;
                tile.VerticalSize = 2;
                _imageTileControl.Groups[0].Tiles.Add(tile);
                try
                {
                    Image img = Image.FromStream(new MemoryStream(imageitem.Base64));
                    Template tl = new Template();
                    ImageElement ie = new ImageElement();
                    ie.ImageLayout = ForeImageLayout.Stretch;
                    tl.Elements.Add(ie);
                    tile.Template = tl;
                    tile.Image = img;
                }
                catch
                {
                    Console.WriteLine("Argument Null Exception");
                }
            }
        }



        //CLICK EVENT OF EXPORT PDF IMAGE
        private void exportImageClick(object sender, EventArgs e)
        {
            List<Image> images = new List<Image>();
            foreach (Tile tile in _imageTileControl.Groups[0].Tiles)
            {
                if (tile.Checked)
                {
                    images.Add(tile.Image);
                }
            }
            //CONVERTING TO PDF
            ConvertToPdf(images);

            //SAVING THE FILE
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "pdf";
            saveFile.Filter = "PDF files (*.pdf)|*.pdf*";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {

                c1PdfDocument1.Save(saveFile.FileName);

            }
           

        }


        //SERVICE FOR CONVERTING IMAGE TO PDF
        private void ConvertToPdf(List<Image> images)
        {
            addPdfDocument();
            RectangleF rect = c1PdfDocument1.PageRectangle;
            bool firstPage = true;
            foreach (var selectedimg in images)
            {
                if (!firstPage)
                {
                    c1PdfDocument1.NewPage();
                }
                firstPage = false;
                rect.Inflate(-72, -72);
                c1PdfDocument1.DrawImage(selectedimg, rect);
            }

        }

        //TIMER EVENT
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            toolStripLabel.Text = DateTime.Now.ToString("dd-MMM-yy hh:mm:ss tt");
            
        }

       

       

        //PAINT EVENT OF EXPORT PDF IMAGE
        private void exportImagePaint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(_exportimage.Location.X, _exportimage.Location.Y, _exportimage.Width, _exportimage.Height);

            r.X -= 29;
            r.Y -= 3;
            r.Width--;
            r.Height--;
            Pen p = new Pen(Color.Black);
            e.Graphics.DrawRectangle(p, r);
            e.Graphics.DrawLine(p, new Point(0, 43), new Point(this.Width, 43));
        }


        //PAINT EVENT OF IMAGE TILE
        private void imageTileControlPaint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawLine(p, 0, 43, 800, 43);
 
        }

        //CHECKED EVENT
        private void imageTileControlTileChecked(object sender, TileEventArgs e)
        {
            checkedItems++;
            _exportimage.Visible = true;
            check.Text = "Checked Images: " + checkedItems.ToString();
        }

        //UNCHECKED EVENT
        private void imageTileControlTileUnchecked(object sender, TileEventArgs e)
        {
            checkedItems--;
            _exportimage.Visible = checkedItems > 0;
            check.Text = "Checked Images: " + checkedItems.ToString();
        }

    }
}

