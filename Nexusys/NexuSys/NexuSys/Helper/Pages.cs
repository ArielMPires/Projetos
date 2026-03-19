namespace NexuSys.Helper
{
    public class Pages
    {
        public string Codigo { get; set; }

        public string Name { get; set; }


        public static List<Pages> pages { get; } = new List<Pages>
        {
            new Pages { Codigo = "Home", Name = "Login" },
            new Pages { Codigo = "ND06", Name = "Dashboard" },
            new Pages { Codigo = "NO01", Name = "Ordens de Serviço" },

            new Pages { Codigo = "NR01", Name = "Relatórios" },
            new Pages { Codigo = "NR02", Name = "Relatórios Técnico" },
            new Pages { Codigo = "NR03", Name = "Relatórios Detalhados" },

            new Pages { Codigo = "NC75", Name = "Cliente" },
            new Pages { Codigo = "NE77", Name = "Editar Cliente" },
            new Pages { Codigo = "NC66", Name = "Novo Cliente" },

            new Pages { Codigo = "NF05", Name = "Fornecedores" },
            new Pages { Codigo = "NE29", Name = " Editar Fornecedores" },
            new Pages { Codigo = "NA92", Name = " Editar Fornecedores" },

            new Pages { Codigo = "NT01", Name = "Tipos de Serviço" },
            new Pages { Codigo = "NT02", Name = "Adicionar Tipo de Serviço" },
            new Pages { Codigo = "NT03", Name = "Editar Serviço" },

            new Pages { Codigo = "NU88", Name = "Usuários" },
            new Pages { Codigo = "NC47", Name = "Cadastrar Usuário" },
            new Pages { Codigo = "NE98", Name = "Editar Usuário" },

            new Pages { Codigo = "NE47", Name = "Novo Equipamento" },
            new Pages { Codigo = "NE21", Name = "Lista de Equipamentos" },
            new Pages { Codigo = "NE12", Name = "Editar Equipamento" },

            new Pages { Codigo = "NP01", Name = "Produtos" },
            new Pages { Codigo = "NP02", Name = "Adicionar Produto" },
            new Pages { Codigo = "NP03", Name = "Editar Produto" },

            new Pages { Codigo = "NO99", Name = "Orçamentos" },
            new Pages { Codigo = "ND98", Name = "Novo Orçamento" },
            new Pages { Codigo = "NE11", Name = "Editar Orçamento" },

            new Pages { Codigo = "ND70", Name = "Departamentos" },
            new Pages { Codigo = "NA44", Name = "Adicionar Departamento" },
            new Pages { Codigo = "NE81", Name = "Editar Departamento" },

            new Pages { Codigo = "NN02", Name = "Notas Fiscais" },
            new Pages { Codigo = "NN01", Name = "Adicionar Nota Fiscal" },
            new Pages { Codigo = "NN03", Name = "Editar Nota Fiscal" },

            new Pages { Codigo = "NP65", Name = "Possíveis Defeitos" },
            new Pages { Codigo = "NA85", Name = "Adicionar Possível Defeito" },
            new Pages { Codigo = "NE22", Name = "Editar Possível Defeito" },

            new Pages { Codigo = "NW01", Name = "Revisão" },
            new Pages { Codigo = "NW02", Name = "Visualização do Equipamento" },
            new Pages { Codigo = "NW03", Name = "Editar Revisão" },
            new Pages { Codigo = "NW04", Name = "Histórico de Revisão do Equipamento" },

            new Pages { Codigo = "NC83", Name = "Compras" },
            new Pages { Codigo = "NC84", Name = "Nova Compra " },
            new Pages { Codigo = "NE22", Name = "Editar Possível Defeito" },

            new Pages { Codigo = "NS01", Name = "Vendedores" },
            new Pages { Codigo = "NS02", Name = "Novo Vendedor " },
            new Pages { Codigo = "NS03", Name = "Editar Vendedores" },

            new Pages { Codigo = "NU57", Name = "Unidades" },
            new Pages { Codigo = "NA60", Name = "Nova Unidades " },
            new Pages { Codigo = "NE40", Name = "Editar Unidades" },

            new Pages { Codigo = "NF01", Name = "Funções" },
            new Pages { Codigo = "NA05", Name = "Nova Funções " },
            new Pages { Codigo = "NE09", Name = "Editar Fuñções" },
            new Pages { Codigo = "NR43", Name = "Permissoes de Funções" },


            new Pages { Codigo = "NS42", Name = "Situações" },
            new Pages { Codigo = "SA55", Name = "Nova Situação " },
            new Pages { Codigo = "SE64", Name = "Editar Situações" },
         
        };
    }
}