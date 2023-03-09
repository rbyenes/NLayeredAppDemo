using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.DataAccess.Concrete.NHibernate;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WebFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _productService = new ProductManager(new EfProductDal());
            _categoryService = new CategoryManager(new EfCategoryDal());
        }

        private IProductService _productService;
        ICategoryService _categoryService;
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProduct();
            LoadCategories();
        }

        private void LoadCategories()
        {
            cbxCategory.DataSource = _categoryService.GetAll();
            cbxCategory.DisplayMember = "CategoryName";
            cbxCategory.ValueMember = "CategoryId";

            cbxCategoryAdd.DataSource = _categoryService.GetAll();
            cbxCategoryAdd.DisplayMember = "CategoryName";
            cbxCategoryAdd.ValueMember = "CategoryId";

            cbxCategoryUpdate.DataSource = _categoryService.GetAll();
            cbxCategoryUpdate.DisplayMember = "CategoryName";
            cbxCategoryUpdate.ValueMember = "CategoryId";
        }

        private void LoadProduct()
        {
            dgwProduct.DataSource = _productService.GetAll();
        }

        private void gbxCategory_Enter(object sender, EventArgs e)
        {

        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgwProduct.DataSource = _productService.GetProductsByCategory(Convert.ToInt32(cbxCategory.SelectedValue));
            }
            catch
            {

            }
        }

        private void tbxProductName_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbxProductName.Text))
            {
                LoadProduct();
            }
            else
            {
                dgwProduct.DataSource = _productService.GetProductsByProductName(tbxProductName.Text);
            }


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            HandleException(() =>
            {
                _productService.Add(new Product
                {
                    ProductName = tbxProductNameAdd.Text,
                    CategoryId = Convert.ToInt32(cbxCategoryAdd.SelectedValue),
                    QuantityPerUnit = tbxQuantity.Text,
                    UnitsInStock = Convert.ToInt16(tbxStock.Text),
                    UnitPrice = Convert.ToDecimal(tbxPrice.Text)
                });
                MessageBox.Show("Eklendi");
                LoadProduct();
            });

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            HandleException(() =>
            {
                _productService.Update(new Product
                {
                    ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value),
                    ProductName = tbxProductNameUpdate.Text,
                    CategoryId = Convert.ToInt32(cbxCategoryUpdate.SelectedValue),
                    UnitPrice = Convert.ToDecimal(tbxPriceUpdate.Text),
                    QuantityPerUnit = tbxQuantityUpdate.Text,
                    UnitsInStock = Convert.ToInt16(tbxStockUpdate.Text)
                });
                MessageBox.Show("Guncellendi");
                LoadProduct();
            });
        }

        private void dgwProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgwProduct.CurrentRow;
            tbxProductNameUpdate.Text = row.Cells[1].Value.ToString();
            cbxCategoryUpdate.SelectedValue = row.Cells[2].Value;
            tbxPriceUpdate.Text = row.Cells[3].Value.ToString();
            tbxQuantityUpdate.Text = row.Cells[4].Value.ToString();
            tbxStockUpdate.Text = row.Cells[5].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            HandleException(() =>                             //action delegesini burada kullanmak istedim
            {
                if (dgwProduct.CurrentRow != null)
                {
                    _productService.Delete(new Product
                    {
                        ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value)
                    });
                    MessageBox.Show("Silindi");
                    LoadProduct();
                }
            });
        }
        private void HandleException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);                      //ProductManagerda exception ismini atadik burada da messagebox olarak gormemi sagliyorum
            }
        }
    }
}