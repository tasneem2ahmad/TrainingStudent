using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Training.DAL.Entities;
using WinFormsApp1.Services;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IdentityService identityService;

        public Form1(IdentityService identityService)
        {
            
            this.userManager = userManager;
            InitializeComponent();
            this.identityService = identityService;
        }

        public async void button1_Click(object sender, EventArgs e)
        {
            var result = await userManager.Users.ToListAsync();
            dataGridView1.DataSource = result;
        }
    }
}
