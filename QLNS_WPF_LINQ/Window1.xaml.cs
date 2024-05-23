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
using System.Windows.Shapes;
using System.Data.Linq;
namespace QLNS_WPF_LINQ
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        QLNSDataContext dc = new QLNSDataContext();
        Table<DSNV> DSNVs;
        Table<CHUCVU> CHUCVUs;
        Table<DMPHONG> DMPHONGs;
        public Window1()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Window1_Load);
            btnTinhToan.Click += new RoutedEventHandler(TinhToan);
        }

        private void TinhToan(object sender, RoutedEventArgs e)
        {
            //Bước 1: Nguồn dữ liệu
            DSNVs = dc.GetTable<DSNV>();
            CHUCVUs = dc.GetTable<CHUCVU>();
            DMPHONGs = dc.GetTable<DMPHONG>();

            //Bước 2: Truy vấn từ 3 bảng
            var query = (from a in DSNVs
                         join b in CHUCVUs
                         on a.MaChucVu equals b.MaChucVu
                         join c in DMPHONGs
                         on a.MaPhong equals c.MaPhong
                         select new
                         {
                             manv = a.MaNV,
                             ht = a.HoTen,
                             ns = a.NgaySinh,
                             gt = a.GioiTinh,
                             luong = a.HeSoLuong * b.PhuCapCV,
                             tenchucvu = b.TenChucVu,
                             tenphong = c.TenPhong
                         }).ToList();
            double sum = 0;
            foreach (var item in query)
                sum += item.luong;
            txtTongLuong.Text = "Tổng lương:" + Math.Round(sum);

        }

        public void load_DSNV()
        {
            //Bước 1: Nguồn dữ liệu
            DSNVs = dc.GetTable<DSNV>();
            CHUCVUs = dc.GetTable<CHUCVU>();
            DMPHONGs = dc.GetTable<DMPHONG>();

            //Bước 2: Truy vấn từ 3 bảng
            var query = (from a in DSNVs
                         join b in CHUCVUs
                         on a.MaChucVu equals b.MaChucVu
                         join c in DMPHONGs
                         on a.MaPhong equals c.MaPhong
                         select new
                         {
                             manv = a.MaNV,
                             ht = a.HoTen,
                             ns = a.NgaySinh,
                             gt = a.GioiTinh,
                             luong = a.HeSoLuong * b.PhuCapCV,
                             tenchucvu = b.TenChucVu,
                             tenphong = c.TenPhong
                         }).ToList();

            //Thực thi truy vấn
            DataGrid2.ItemsSource = query;

           
        }
        private void Window1_Load(object sender, RoutedEventArgs e)
        {
            load_DSNV();
        }

    }
}
