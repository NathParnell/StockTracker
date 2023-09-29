using StockTracker.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockTracker
{
    public partial class frmStockTracker : Form
    {
        private ctrLogin _ctrLogin;

        public frmStockTracker(ctrLogin ctrLogin)
        {
            InitializeComponent();

            _ctrLogin = ctrLogin;
        }

        private void frmStockTracker_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            GenerateLoginControl();
        }

        private void GenerateLoginControl()
        {
            ClearPanel();

            //position the control towards the top of the panel
            _ctrLogin.Left = (pnlContent.Width - _ctrLogin.Width) / 2;
            _ctrLogin.Top = (pnlContent.Height - _ctrLogin.Height) / 3;

            pnlContent.Controls.Add(_ctrLogin);
        }

        private void ClearPanel()
        {
            if (pnlContent.Controls.Count > 0)
            {
                pnlContent.Controls.Clear();
            }
        }

    }
}
