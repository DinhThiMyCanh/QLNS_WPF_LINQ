using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.Data;

namespace QLNS_WPF_LINQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        QLNSDataContext dc = new QLNSDataContext();
        Table<DMPHONG> DMPHONGs;
        Table<CHUCVU> CHUCVUs;
        Table<DSNV> DSNVs;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Windows_Load);

            btnThem.Click += new RoutedEventHandler(them);
            btnSua.Click += new RoutedEventHandler(sua);
            btnXoa.Click += new RoutedEventHandler(xoa);

            DataGrid.SelectionChanged += new SelectionChangedEventHandler(Data_Click);
        }

        //Sự kiện  buộc dữ liệu từ DataGrid lên các control
        private void Data_Click(object sender, SelectionChangedEventArgs e)
        {
            DSNV nv = DataGrid.SelectedItem as DSNV; //Nhân viên được chọn trong DataGrid
            if (nv!=null)
            {
                txtMaNV.Text = nv.MaNV.ToString();
                txtHoTen.Text = nv.HoTen.ToString();
                dtpNgaySinh.Text = nv.NgaySinh.ToString();
                string gt = nv.GioiTinh.ToString();
                if (gt.Equals("True"))
                    rdNam.IsChecked = true;
                else
                    rdNu.IsChecked = true;
                txtSoDT.Text = nv.SoDT.ToString();
                txtHSL.Text = nv.HeSoLuong.ToString();
                cboTenPhong.SelectedValue = nv.MaPhong.ToString();
                cboChucVu.SelectedValue = nv.MaChucVu.ToString();
            }    
        }

        private void xoa(object sender, RoutedEventArgs e)
        {
            var query = from nv in DSNVs
                        where nv.MaNV == int.Parse(txtMaNV.Text)
                        select nv;
            foreach (var nv in query)
            {
                DSNVs.DeleteOnSubmit(nv);   
            }
            dc.SubmitChanges();
            loadDSNV();
        }

        private void sua(object sender, RoutedEventArgs e)
        {
            var query = from nv in DSNVs
                        where nv.MaNV == int.Parse(txtMaNV.Text)
                        select nv;
            foreach (var nv in query)
            {
                nv.HoTen = txtHoTen.Text;
                nv.NgaySinh = Convert.ToDateTime(dtpNgaySinh.Text);
                nv.GioiTinh = rdNam.IsChecked == true ? true : false;
                nv.SoDT = txtSoDT.Text;
                nv.HeSoLuong = float.Parse(txtHSL.Text);
                nv.MaPhong = cboTenPhong.SelectedValue.ToString();
                nv.MaChucVu = cboChucVu.SelectedValue.ToString();
                dc.SubmitChanges();
            }
           
            loadDSNV();
        }

        private void them(object sender, RoutedEventArgs e)
        {
            DSNV nv = new DSNV();
            nv.HoTen = txtHoTen.Text;
            nv.NgaySinh = Convert.ToDateTime(dtpNgaySinh.Text);
            nv.GioiTinh = rdNam.IsChecked == true ? true : false;
            nv.SoDT = txtSoDT.Text;
            nv.HeSoLuong = float.Parse(txtHSL.Text);
            nv.MaPhong = cboTenPhong.SelectedValue.ToString();
            nv.MaChucVu = cboChucVu.SelectedValue.ToString();

            //Thêm nv vào Entity DSNVs
            DSNVs.InsertOnSubmit(nv);
            //Cập nhật dữ liệu xuống Database
            dc.SubmitChanges();
            loadDSNV();
        }
        #region load dữ liệu
        public void loadPB()
        {
            //Bước 1: Nguồn dữ liệu
            DMPHONGs = dc.GetTable<DMPHONG>();

            //Bước 2: Câu lệnh truy vấn LINQ
            var query = from pb in DMPHONGs
                        select new {mapb = pb.MaPhong, tenpb = pb.TenPhong };
            //Bước 3: Thực thi truy vấn LINQ
            cboTenPhong.ItemsSource = query;
            cboTenPhong.DisplayMemberPath = "tenpb";
            cboTenPhong.SelectedValuePath = "mapb";
        }
        public void loadChucVu()
        {
            CHUCVUs = dc.GetTable<CHUCVU>();
            var query = from cv in CHUCVUs
                        select new {macv = cv.MaChucVu, tencv = cv.TenChucVu};

            cboChucVu.ItemsSource = query;
            cboChucVu.DisplayMemberPath = "tencv";
            cboChucVu.SelectedValuePath = "macv";
        }
        public void loadDSNV()
        {
            DSNVs = dc.GetTable<DSNV>();
            var query = from nv in DSNVs
                        select nv;
            DataGrid.ItemsSource = query;
        }
        #endregion
        private void Windows_Load(object sender, RoutedEventArgs e)
        {
            loadPB();
            loadChucVu();
            loadDSNV();
        }
    }
}
