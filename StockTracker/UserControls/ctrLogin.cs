using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using StockTrackerCommon.Services;
using StockTrackerCommon.Services.Infrastructure;

namespace StockTracker.UserControls
{
    public partial class ctrLogin : UserControl
    {
        private readonly IUserService _userService;

        public ctrLogin(IUserService userService)
        {
            InitializeComponent();

            _userService = userService;
        }

        public void Init()
        {
            txtUsername.Text = String.Empty;
            txtPassword.Text = String.Empty;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsername.Text) || String.IsNullOrEmpty(txtPassword.Text))
            {
                //we need like an error message to say that they have not entered a value
                // ...
                return;
            }

            User user = _userService.RequestLogin(txtUsername.Text, txtPassword.Text);


        }
    }
}
