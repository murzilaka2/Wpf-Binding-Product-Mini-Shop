using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Product> cartProducts;
        private DataSource dataSource;
        public MainWindow()
        {
            InitializeComponent();
            cartProducts = new List<Product>();
            dataSource = new DataSource();
            this.DataContext = dataSource;
        }

        private void cartBUtton_Click(object sender, RoutedEventArgs e)
        {
            decimal total = 0;
            var st = new StringBuilder();
            foreach (Product product in cartProducts)
            {
                st.AppendLine($"Name: {product.Name}, count: {product.Count}, total: {product.Price * product.Count}");
                total += product.Price * product.Count;
            }
            st.AppendLine($"{Environment.NewLine}Total: {total}");
            MessageBox.Show(st.ToString(), "Total to pay");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Product selectedProduct)
            {
                dataSource.TotalSum += selectedProduct.Price;
                dataSource.ProductsCount++;
                selectedProduct.Count--;

                if (cartProducts.Any(e => e.Name.Equals(selectedProduct.Name)))
                {
                    cartProducts.FirstOrDefault(e=>e.Name.Equals(selectedProduct.Name)).Count++;
                }
                else
                {
                    cartProducts.Add(new Product
                    {
                        Count = 1,
                        Name = selectedProduct.Name,
                        Price = selectedProduct.Price
                    });
                }

            }
        }
    }
    class DataSource : INotifyPropertyChanged
    {
        public IEnumerable<Product> Products { get; set; }  
        private decimal totalSum { get; set; }
        public decimal TotalSum
        {
            get
            {
                return totalSum;
            }
            set
            {
                totalSum = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(TotalSum)));
            }
        }

        private int productsCount { get; set; }
        public int ProductsCount
        {
            get
            {
                return productsCount;
            }
            set
            {
                productsCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ProductsCount)));
            }
        }

        private string textTotalSum;

        public DataSource()
        {
            Products = new[]
            {
                    new Product{ Name = "Apple", Count = 50, Price = 15, ImageUrl = "/Images/apple.png"},
                    new Product{ Name = "Orange", Count = 123, Price = 55, ImageUrl = "/Images/orange.png"},
                    new Product{ Name = "Cherry", Count = 76, Price = 75, ImageUrl = "/Images/cherry.png"}
                };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }

    class Product : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        private decimal count;
        public decimal Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            }
        }
        public string ImageUrl { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}