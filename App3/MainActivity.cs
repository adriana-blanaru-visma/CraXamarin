using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Linq;
namespace App3
{
    [Activity(Label = "CRApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it

            TextView txtUser = FindViewById<TextView>(Resource.Id.txtUser);
            TextView txtManager = FindViewById<TextView>(Resource.Id.txtManager);
            TextView txtProd = FindViewById<TextView>(Resource.Id.txtProd);
            TextView txtPrice = FindViewById<TextView>(Resource.Id.txtPrice);
            TextView txtResult = FindViewById<TextView>(Resource.Id.txtResult);
            ListView listView1 = FindViewById<ListView>(Resource.Id.listView1);
            ScrollView scrollView1 = FindViewById<ScrollView>(Resource.Id.scrollView1);

            Button btnInsertCR = FindViewById<Button>(Resource.Id.btnInsertCR);

            btnInsertCR.Click += delegate { txtResult.Text = CRLogic.InsertCR(txtUser.Text, txtManager.Text, txtProd.Text, decimal.Parse(txtPrice.Text)) ? "Success!" : "Error!"; };

            Button btnGetCRs = FindViewById<Button>(Resource.Id.btnGetCRs);

            btnGetCRs.Click += delegate
            {
                var lst = CRLogic.GetAllCRs();
                if (lst != null && lst.Count > 0)
                {
                    var arr = lst.Select(x => string.Format("{0} - {1} - {2} - {3}", x.UserName, x.ManagerName, x.ProductName, x.ProductPrice.ToString())).ToArray();
                    ArrayAdapter<string> aa = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, arr);
                    listView1.Adapter = aa;
                }
            };
        }
    }
}

