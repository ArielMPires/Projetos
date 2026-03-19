using Agnus.ViewModels;
using Agnus.Models;
using Agnus.Views;
using Newtonsoft.Json;
using Agnus.Helpers;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace Agnus
{
    public partial class AppShell : Shell
    {
        private Credencial _credencial;

        public AppShell()
        {
            InitializeComponent();
        }
        public async void UpdateCredencial()
        {
            this.Items.Clear();
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (userBasicInfoStr != null)
            {
                bool tabExists = this.Items
                    .OfType<Tab>()
                    .Any(t => t.Title == "Dashboard");

                if (!tabExists)
                {
                    var dashTab = new Tab
                    {
                        Title = "Dashboard"
                    };

                    Routing.RegisterRoute("SD001", typeof(SD001));

                    dashTab.Items.Add(new ShellContent
                    {
                        Title = "Dashboard",
                        Route = "SD001",
                        ContentTemplate = new DataTemplate(typeof(SD001))
                    });

                    this.Items.Add(dashTab);
                }
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                BuildDynamicTabs();
            }
        }

        private void BuildDynamicTabs()
        {

            // ========== Chamados ==========
            var chamadosTab = new Tab { Title = "Chamados" };
            AddIfAllowed(chamadosTab, "SC001", "Chamados Internos", typeof(Views.SC001));
            AddIfAllowedNoRoute(chamadosTab, "Chamados Prestadores");
            AddIfTabNotEmpty(chamadosTab);

            // ========== Serviços ==========
            var servicosTab = new Tab { Title = "Serviços" };
            AddIfAllowed(servicosTab, "SM001", "Serviços", typeof(Views.SM001));
            AddIfAllowedNoRoute(servicosTab, "Projetos");
            AddIfAllowedNoRoute(servicosTab, "Manutenção");
            AddIfTabNotEmpty(servicosTab);

            // ========== Estoque ==========
            var estoqueTab = new Tab { Title = "Estoque" };
            AddIfAllowed(estoqueTab, "SK001", "Estoque", typeof(Views.SK001));
            AddIfAllowed(estoqueTab, "SK538", "Produtos", typeof(Views.SK538));
            AddIfAllowedNoRoute(estoqueTab, "Inventário");
            AddIfAllowed(estoqueTab, "SK472", "Categorias", typeof(Views.SK472));
            AddIfAllowed(estoqueTab, "SK872", "Marcas", typeof(Views.SK872));
            AddIfTabNotEmpty(estoqueTab);

            // ========== Fornecedores ==========
            var fornecedoresTab = new Tab { Title = "Fornecedores" };
            AddIfAllowed(fornecedoresTab, "SP001", "Fornecedores", typeof(Views.SP001));
            AddIfTabNotEmpty(fornecedoresTab);

            // ========== Compras ==========
            var comprasTab = new Tab { Title = "Compras" };
            AddIfAllowed(comprasTab, "SH001", "Compras", typeof(Views.SH001));
            AddIfAllowed(comprasTab, "SH261", "Solicitações", typeof(Views.SH261));
            AddIfAllowed(comprasTab, "SH853", "Reposição", typeof(Views.SH853));
            AddIfTabNotEmpty(comprasTab);

            // ========== Financeiro ==========
            var financeiroTab = new Tab { Title = "Financeiro" };
            AddIfAllowed(financeiroTab, "SF001", "Financeiro", typeof(Views.SF001));
            AddIfAllowed(financeiroTab, "SF852", "Movimentações", typeof(Views.SF852));
            AddIfAllowedNoRoute(financeiroTab, "Consultas");
            AddIfTabNotEmpty(financeiroTab);

            // ========== Administrativo ==========
            var administrativoTab = new Tab { Title = "Administrativo" };
            AddIfAllowed(administrativoTab, "SA001", "Administrativo", typeof(Views.SA001));
            AddIfAllowed(administrativoTab, "SA961", "Patrimônio", typeof(Views.SA961));
            AddIfAllowed(administrativoTab, "SA803", "Computadores", typeof(Views.SA803));
            AddIfAllowed(administrativoTab,"SA221", "Prestadores", typeof(Views.SA221));
            AddIfAllowed(administrativoTab,"SA278", "Manuais",typeof(Views.SA278));
            AddIfAllowed(administrativoTab,"SA559", "Senhas",typeof(Views.SA559));
            AddIfTabNotEmpty(administrativoTab);

            // ========== Gerencial ==========
            var gerencialTab = new Tab { Title = "Gerencial" };
            AddIfAllowed(gerencialTab, "SG001", "Gerencial", typeof(Views.SG001));
            AddIfAllowedNoRoute(gerencialTab, "Relatórios");
            AddIfAllowed(gerencialTab, "SG429", "Tipos de Serviços", typeof(Views.SG429));
            AddIfAllowed(gerencialTab, "SG737", "Tipo/Categoria de Chamados", typeof(Views.SG737));
            AddIfAllowed(gerencialTab, "SG897", "Usos", typeof(Views.SG897));
            AddIfAllowed(gerencialTab, "SG351", "Check-List", typeof(Views.SG351));
            AddIfTabNotEmpty(gerencialTab);

            // ========== Configurações ==========
            var configTab = new Tab { Title = "Configurações" };
            AddIfAllowed(configTab, "SS001", "Usuários", typeof(Views.SS001));
            AddIfAllowed(configTab, "SS328", "Departamentos", typeof(Views.SS328));
            AddIfAllowed(configTab, "SS573", "Funções", typeof(Views.SS573));
            AddIfTabNotEmpty(configTab);
        }

        // Para itens com route definido
        private void AddIfAllowed(Tab tab, string route, string title, Type viewType)
        {
            if (_credencial.Permissions.FirstOrDefault(e => e.Page == route) != null)
            {
                Routing.RegisterRoute(route, viewType);
                tab.Items.Add(new ShellContent
                {
                    Route = route,
                    Title = title,
                    ContentTemplate = new DataTemplate(viewType)
                });
            }
        }

        // Para itens sem route definido
        private void AddIfAllowedNoRoute(Tab tab, string title)
        {
            if (_credencial.Permissions.FirstOrDefault(e => e.Page == title) != null) // aqui a permissão é o título
            {
                tab.Items.Add(new ShellContent
                {
                    Title = title
                });
            }
        }

        private void AddIfTabNotEmpty(Tab tab)
        {
            if (tab.Items.Count > 0)
                this.Items.Add(tab);
        }
    }
}
