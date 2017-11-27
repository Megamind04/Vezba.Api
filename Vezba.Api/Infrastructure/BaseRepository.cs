using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Vezba.Api.Interfaces;
using Vezba.Entity;

namespace Vezba.Api.Infrastructure
{
    abstract class BaseRepository<T> : ITheRingWhoGonnaRuleThemAll<T> /*where T : BaseRepository<T>*/
    {
        private SqlConnection GetSqlConnection()
        {
            SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "Vezba",
                IntegratedSecurity = true,
                AsynchronousProcessing = true
            };

            SqlConnection myConn = new SqlConnection(myBuilder.ConnectionString);
            return myConn;
        }
        
        protected static bool getAllActive, getByIdActive, getCreateActive, getDeleteActive, getEditActive;

        protected abstract String Query { get; }

        protected abstract IQueryable<T> CallBackRead(SqlDataReader myReader);
        protected abstract T CallBackReadID(SqlDataReader myReader);
        protected abstract void CallBackEditorCreate(SqlCommand myComm, T obj);

        public async Task<IQueryable<T>> GetAll()
        {
            try
            {
                using (SqlConnection myConn = GetSqlConnection())
                {
                    getAllActive = true;
                    string SqlQuery = Query;
                    getAllActive = false;
                    await myConn.OpenAsync();
                    using (SqlCommand myComm = new SqlCommand(SqlQuery, myConn))
                    {
                        using (SqlDataReader myReader = await myComm.ExecuteReaderAsync())
                        {
                            return await Task.Run(() => CallBackRead(myReader));
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new Exception("Sql Problem", e);
            }
        }

        public async Task<T> GetById(int Id)
        {
            try
            {
                using (SqlConnection myConn = GetSqlConnection())
                {
                    getByIdActive = true;
                    await myConn.OpenAsync();
                    string SqlQuery = Query;
                    getByIdActive = false;
                    using (SqlCommand myComm = new SqlCommand(SqlQuery, myConn))
                    {
                        myComm.Parameters.AddWithValue("@Id", Id);
                        using (SqlDataReader myReader = await myComm.ExecuteReaderAsync())
                        {
                            return await Task.Run(()=> CallBackReadID(myReader));
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new Exception("Sql Problem", e);
            }
        }

        public async Task<bool> Create(T newCreate)
        {
            try
            {
                using (SqlConnection myConn = GetSqlConnection())
                {
                    getCreateActive = true;
                    await myConn.OpenAsync();
                    string SqlQuery = Query;
                    using (SqlCommand myComm = new SqlCommand(SqlQuery, myConn))
                    {
                        CallBackEditorCreate(myComm, newCreate);
                        await myComm.ExecuteNonQueryAsync();
                        getCreateActive = false;
                        return true;
                    }
                }
            }
            catch (SqlException e)
            {
                return false;
                throw new Exception("Sql Problem", e);
            }
        }

        public async Task<bool> Edit(T edit)
        {
            try
            {
                using (SqlConnection myConn = GetSqlConnection())
                {
                    getEditActive = true;
                    await myConn.OpenAsync();
                    string SqlQuery = Query;
                    using (SqlCommand myComm = new SqlCommand(SqlQuery, myConn))
                    {
                        CallBackEditorCreate(myComm, edit);
                        await myComm.ExecuteNonQueryAsync();
                        getEditActive = false;
                        return true;
                    }
                }
            }
            catch (SqlException e)
            {
                return false;
                throw new Exception("Sql Problem", e);
            }
        }

        public async Task<bool> Delete(int Id)
        {
            try
            {
                using (SqlConnection myConn = GetSqlConnection())
                {
                    getDeleteActive = true;
                    await myConn.OpenAsync();
                    string SqlQuery = Query;
                    using (SqlCommand myComm = new SqlCommand(SqlQuery, myConn))
                    {
                        myComm.Parameters.AddWithValue("@Id", Id);
                        await myComm.ExecuteNonQueryAsync();
                        getDeleteActive = false;
                        return true;
                    }
                }
            }
            catch (SqlException e)
            {
                return false;
                throw new Exception("Sql Problem", e);
            }
        }
    }

    class EmployeeRepository : BaseRepository<Employee>
    {
        private List<Employee> Employees = new List<Employee>();

        protected override string Query
        {
            get
            {
                if (getAllActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.Append("SELECT * FROM Employees");
                    return sB.ToString();
                }
                else if (getByIdActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("SELECT * FROM Employees");
                    sB.Append("WHERE EmployeeID = @Id");
                    return sB.ToString();
                }
                else if (getCreateActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("INSERT INTO Employees(LastName,FirstName,BirthDate,Photo,Notes)");
                    sB.Append("VALUES(@LastName,@FirstName,@BirthDate,@Photo,@Notes)");
                    return sB.ToString();
                }
                else if (getDeleteActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("DELETE FROM Employees");
                    sB.Append("WHERE EmployeeID = @Id");
                    return sB.ToString();
                }
                else if (getEditActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("UPDATE Employees SET ");
                    sB.AppendLine("LastName = @LastName,FirstName = @FirstName,BirthDate = @BirthDate,Photo = @Photo,Notes = @Notes");
                    sB.Append("WHERE EmployeeID = @EmployeeID");
                    return sB.ToString();
                }
                return null;
            }
        }

        protected override void CallBackEditorCreate(SqlCommand myComm, Employee emp)
        {
            if (getCreateActive)
            {
                //Employee emp = obj as Employee;
                myComm.Parameters.AddWithValue("@LastName", emp.LastName);
                myComm.Parameters.AddWithValue("@FirstName", emp.FirstName);
                myComm.Parameters.AddWithValue("@BirthDate", emp.BirthDate);
                myComm.Parameters.AddWithValue("@Photo", emp.Photo);
                myComm.Parameters.AddWithValue("@Notes", emp.Notes);
            }
            else if (getEditActive)
            {
                //Employee emp = obj as Employee;
                myComm.Parameters.AddWithValue("@EmployeeID", emp.EmployeeID);
                myComm.Parameters.AddWithValue("@LastName", emp.LastName);
                myComm.Parameters.AddWithValue("@FirstName", emp.FirstName);
                myComm.Parameters.AddWithValue("@BirthDate", emp.BirthDate);
                myComm.Parameters.AddWithValue("@Photo", emp.Photo);
                myComm.Parameters.AddWithValue("@Notes", emp.Notes);
            }
        }
        protected override IQueryable<Employee> CallBackRead(SqlDataReader myReader)
        {
            foreach (var item in myReader)
            {
                Employees.Add(new Employee()
                {
                    EmployeeID = myReader.GetInt32(0),
                    LastName = myReader.GetString(1),
                    FirstName = myReader.GetString(2),
                    BirthDate = myReader.GetDateTime(3),
                    Photo = myReader.GetString(4),
                    Notes = myReader.GetString(5)
                });
            }
            return Employees.AsQueryable();
        }
        protected override Employee CallBackReadID(SqlDataReader myReader)
        {
            myReader.ReadAsync();
            Employee emp = new Employee()
            {
                EmployeeID = myReader.GetInt32(0),
                LastName = myReader.GetString(1),
                FirstName = myReader.GetString(2),
                BirthDate = myReader.GetDateTime(3),
                Photo = myReader.GetString(4),
                Notes = myReader.GetString(5)
            };
            return emp;
        }
    }

    class CustomerRepository : BaseRepository<Customer>
    {
        private List<Customer> Customers = new List<Customer>();

        protected override string Query
        {
            get
            {
                if (getAllActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.Append("SELECT * FROM Customers");
                    return sB.ToString();
                }
                else if (getByIdActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("SELECT * FROM Customers");
                    sB.Append("WHERE CustomerID = @Id");
                    return sB.ToString();
                }
                else if (getCreateActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("INSERT INTO Customers(CustomerName,ContactName,Address,City,PostalCode,Country)");
                    sB.Append("VALUES(@CustomerName,@ContactName,@Address,@City,@PostalCode,@Country)");
                    return sB.ToString();
                }
                else if (getDeleteActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("DELETE FROM Customers");
                    sB.Append("WHERE CustomerID = @Id");
                    return sB.ToString();
                }
                else if (getEditActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("UPDATE Customers SET ");
                    sB.AppendLine("CustomerName = @CustomerName,ContactName = @ContactName,Address = @Address,City = @City,PostalCode = @PostalCode,Country = @Country");
                    sB.Append("WHERE CustomerID = @EmployeeID");
                    return sB.ToString();
                }
                return null;
            }
        }

        protected override void CallBackEditorCreate(SqlCommand myComm, Customer cus)
        {
            if (getCreateActive)
            {
                //Customer cus = obj as Customer;
                myComm.Parameters.AddWithValue("@CustomerName", cus.CustomerName);
                myComm.Parameters.AddWithValue("@ContactName", cus.ContactName);
                myComm.Parameters.AddWithValue("@Address", cus.Address);
                myComm.Parameters.AddWithValue("@City", cus.City);
                myComm.Parameters.AddWithValue("@PostalCode", cus.PostalCode);
                myComm.Parameters.AddWithValue("@Country", cus.Country);
            }
            else if (getEditActive)
            {
                //Customer cus = obj as Customer;
                myComm.Parameters.AddWithValue("@CustomerID", cus.CustomerID);
                myComm.Parameters.AddWithValue("@CustomerName", cus.CustomerName);
                myComm.Parameters.AddWithValue("@ContactName", cus.ContactName);
                myComm.Parameters.AddWithValue("@Address", cus.Address);
                myComm.Parameters.AddWithValue("@City", cus.City);
                myComm.Parameters.AddWithValue("@PostalCode", cus.PostalCode);
                myComm.Parameters.AddWithValue("@Country", cus.Country);
            }
        }
        protected override IQueryable<Customer> CallBackRead(SqlDataReader myReader)
        {
            foreach (var item in myReader)
            {
                Customers.Add(new Customer()
                {
                    CustomerID = myReader.GetInt32(0),
                    CustomerName = myReader.GetString(1),
                    ContactName = myReader.GetString(2),
                    Address = myReader.GetString(3),
                    City = myReader.GetString(4),
                    PostalCode = myReader.GetString(5),
                    Country = myReader.GetString(6)
                });
            }
            return Customers.AsQueryable();
        }
        protected override Customer CallBackReadID(SqlDataReader myReader)
        {
            myReader.ReadAsync();
            Customer cus = new Customer()
            {
                CustomerID = myReader.GetInt32(0),
                CustomerName = myReader.GetString(1),
                ContactName = myReader.GetString(2),
                Address = myReader.GetString(3),
                City = myReader.GetString(4),
                PostalCode = myReader.GetString(5),
                Country = myReader.GetString(6)
            };
            return cus;
        }
    }

    class ShipperRepository : BaseRepository<Shipper>
    {
        private List<Shipper> Shippers = new List<Shipper>();

        protected override string Query
        {
            get
            {
                if (getAllActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.Append("SELECT * FROM Shippers");
                    return sB.ToString();
                }
                else if (getByIdActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("SELECT * FROM Shippers");
                    sB.Append("WHERE ShipperID = @Id");
                    return sB.ToString();
                }
                else if (getCreateActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("INSERT INTO Shippers(ShipperName,Phone)");
                    sB.Append("VALUES(@ShipperName,@Phone)");
                    return sB.ToString();
                }
                else if (getDeleteActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("DELETE FROM Shippers");
                    sB.Append("WHERE ShipperID = @Id");
                    return sB.ToString();
                }
                else if (getEditActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("UPDATE Shippers SET ");
                    sB.AppendLine("ShipperName = @ShipperName,Phone = @Phone");
                    sB.Append("WHERE ShipperID = @ShipperID");
                    return sB.ToString();
                }
                return null;
            }
        }

        protected override void CallBackEditorCreate(SqlCommand myComm, Shipper shi)
        {
            if (getCreateActive)
            {
                //Shipper shi = obj as Shipper;
                myComm.Parameters.AddWithValue("@ShipperName", shi.ShipperName);
                myComm.Parameters.AddWithValue("@Phone", shi.Phone);
            }
            else if (getEditActive)
            {
                //Shipper shi = obj as Shipper;
                myComm.Parameters.AddWithValue("@ShipperID", shi.ShipperID);
                myComm.Parameters.AddWithValue("@ShipperName", shi.ShipperName);
                myComm.Parameters.AddWithValue("@Phone", shi.Phone);               
            }
        }
        protected override IQueryable<Shipper> CallBackRead(SqlDataReader myReader)
        {
            foreach (var item in myReader)
            {
                Shippers.Add(new Shipper()
                {
                    ShipperID = myReader.GetInt32(0),
                    ShipperName = myReader.GetString(1),
                    Phone = myReader.GetString(2)
                });
            }
            return Shippers.AsQueryable();
        }
        protected override Shipper CallBackReadID(SqlDataReader myReader)
        {
            myReader.ReadAsync();
            Shipper shi = new Shipper()
            {
                ShipperID = myReader.GetInt32(0),
                ShipperName = myReader.GetString(1),
                Phone = myReader.GetString(2)
            };
            return shi;
        }
    }

    class CategoryRepository : BaseRepository<Category>
    {
        private List<Category> Categories = new List<Category>();

        protected override string Query
        {
            get
            {
                if (getAllActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.Append("SELECT * FROM Categories");
                    return sB.ToString();
                }
                else if (getByIdActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("SELECT * FROM Categories");
                    sB.Append("WHERE CategoryID = @Id");
                    return sB.ToString();
                }
                else if (getCreateActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("INSERT INTO Categories(CategoryName,Description)");
                    sB.Append("VALUES(@CategoryName,@Description)");
                    return sB.ToString();
                }
                else if (getDeleteActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("DELETE FROM Categories");
                    sB.Append("WHERE CategoryID = @Id");
                    return sB.ToString();
                }
                else if (getEditActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("UPDATE Categories SET ");
                    sB.AppendLine("CategoryName = @CategoryName,Description = @Description");
                    sB.Append("WHERE CategoryID = @CategoryID");
                    return sB.ToString();
                }
                return null;
            }
        }

        protected override void CallBackEditorCreate(SqlCommand myComm, Category cat)
        {
            if (getCreateActive)
            {
                //Shipper shi = obj as Shipper;
                myComm.Parameters.AddWithValue("@CategoryName", cat.CategoryName);
                myComm.Parameters.AddWithValue("@Description", cat.Description);
            }
            else if (getEditActive)
            {
                //Shipper shi = obj as Shipper;
                myComm.Parameters.AddWithValue("@CategoryID", cat.CategoryID);
                myComm.Parameters.AddWithValue("@CategoryName", cat.CategoryName);
                myComm.Parameters.AddWithValue("@Description", cat.Description);
            }
        }
        protected override IQueryable<Category> CallBackRead(SqlDataReader myReader)
        {
            foreach (var item in myReader)
            {
                Categories.Add(new Category()
                {
                    CategoryID = myReader.GetInt32(0),
                    CategoryName = myReader.GetString(1),
                    Description = myReader.GetString(2)
                });
            }
            return Categories.AsQueryable();
        }
        protected override Category CallBackReadID(SqlDataReader myReader)
        {
            myReader.ReadAsync();
            Category cat = new Category()
            {
                CategoryID = myReader.GetInt32(0),
                CategoryName = myReader.GetString(1),
                Description = myReader.GetString(2)
            };
            return cat;
        }
    }

    class SupplierRepository : BaseRepository<Supplier>
    {
        private List<Supplier> Suppliers = new List<Supplier>();

        protected override string Query
        {
            get
            {
                if (getAllActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.Append("SELECT * FROM Suppliers");
                    return sB.ToString();
                }
                else if (getByIdActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("SELECT * FROM Suppliers");
                    sB.Append("WHERE SupplierID = @Id");
                    return sB.ToString();
                }
                else if (getCreateActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("INSERT INTO Suppliers(SupplierName,ContactName,Address,City,PostalCode,Country,Phone)");
                    sB.Append("VALUES(@SupplierName,@ContactName,@Address,@City,@PostalCode,@Country,@Phone)");
                    return sB.ToString();
                }
                else if (getDeleteActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("DELETE FROM Suppliers");
                    sB.Append("WHERE SupplierID = @Id");
                    return sB.ToString();
                }
                else if (getEditActive)
                {
                    StringBuilder sB = new StringBuilder();
                    sB.AppendLine("UPDATE Suppliers SET ");
                    sB.AppendLine("SupplierName = @SupplierName,ContactName = @ContactName,Address = @Address,City = @City,PostalCode = @PostalCode,Country = @Country,Phone = @Phone");
                    sB.Append("WHERE SupplierID = @SupplierID");
                    return sB.ToString();
                }
                return null;
            }
        }

        protected override void CallBackEditorCreate(SqlCommand myComm, Supplier sup)
        {
            if (getCreateActive)
            {
                //Shipper shi = obj as Shipper;
                myComm.Parameters.AddWithValue("@SupplierName", sup.SupplierName);
                myComm.Parameters.AddWithValue("@ContactName", sup.ContactName);
                myComm.Parameters.AddWithValue("@Address", sup.Address);
                myComm.Parameters.AddWithValue("@City", sup.City);
                myComm.Parameters.AddWithValue("@PostalCode", sup.PostalCode);
                myComm.Parameters.AddWithValue("@Country", sup.Country);
                myComm.Parameters.AddWithValue("@Phone", sup.Phone);
            }
            else if (getEditActive)
            {
                //Shipper shi = obj as Shipper;
                myComm.Parameters.AddWithValue("@SupplierID", sup.SupplierID);
                myComm.Parameters.AddWithValue("@SupplierName", sup.SupplierName);
                myComm.Parameters.AddWithValue("@ContactName", sup.ContactName);
                myComm.Parameters.AddWithValue("@Address", sup.Address);
                myComm.Parameters.AddWithValue("@City", sup.City);
                myComm.Parameters.AddWithValue("@PostalCode", sup.PostalCode);
                myComm.Parameters.AddWithValue("@Country", sup.Country);
                myComm.Parameters.AddWithValue("@Phone", sup.Phone);
            }
        }
        protected override IQueryable<Supplier> CallBackRead(SqlDataReader myReader)
        {
            foreach (var item in myReader)
            {
                Suppliers.Add(new Supplier()
                {
                    SupplierID = myReader.GetInt32(0),
                    SupplierName = myReader.GetString(1),
                    ContactName = myReader.GetString(2),
                    Address = myReader.GetString(3),
                    City = myReader.GetString(4),
                    PostalCode = myReader.GetString(5),
                    Country = myReader.GetString(6),
                    Phone = myReader.GetString(7)
                });
            }
            return Suppliers.AsQueryable();
        }
        protected override Supplier CallBackReadID(SqlDataReader myReader)
        {
            myReader.ReadAsync();
            Supplier sup = new Supplier()
            {
                SupplierID = myReader.GetInt32(0),
                SupplierName = myReader.GetString(1),
                ContactName = myReader.GetString(2),
                Address = myReader.GetString(3),
                City = myReader.GetString(4),
                PostalCode = myReader.GetString(5),
                Country = myReader.GetString(6),
                Phone = myReader.GetString(7)
            };
            return sup;
        }
    }
}