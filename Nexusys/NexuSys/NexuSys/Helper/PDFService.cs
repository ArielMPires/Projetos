using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using NexuSys.Data;
using NexuSys.DTOs.ServiceOrder;
using Org.BouncyCastle.Asn1.X509;
using System.Threading.Tasks;

namespace NexuSys.Helper
{
    public class PDFService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public PDFService(IWebHostEnvironment env, ApplicationDbContext context)
        {
            _env = env;
            _context = context;
        }

        public async Task<string> GerarOrçamentoAssistencia(OSDetails orcamentoDados)
        {

            string? opt = null;

            if (orcamentoDados.EquipamentFK.Optional != null)
                opt = orcamentoDados.EquipamentFK.Optional;
            else
                opt = "Sem Opcionais";

            string filename = $"{orcamentoDados.BudgetFK.ID}.pdf";
            var basePath = Path.Combine(
                _env.WebRootPath,
                "PDFOrcamento"
            );

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            if (File.Exists($"{basePath}/{filename}"))
                return $"{filename}";
            Document doc = new Document(PageSize.A4, 20, 20, 20, 20);
            PdfWriter.GetInstance(doc, new FileStream($"{basePath}/{filename}", FileMode.Create));
            doc.Open();

            // ================= FONTES =================
            BaseFont bfRegular = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            BaseFont bfBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            Font normal = new Font(bfRegular, 9);
            Font bold = new Font(bfBold, 9);
            Font header = new Font(bfBold, 10);
            Font tituloFont = new Font(bfBold, 16, Font.NORMAL, BaseColor.RED);

            BaseColor cinzaClaro = new BaseColor(240, 240, 240);
            BaseColor cinzaHeader = new BaseColor(220, 220, 220);
            BaseColor cinzaBorda = new BaseColor(200, 200, 200);

            #region CABEÇALHO PRINCIPAL DA PÁGINA COM (LOGO, RELATÓRIO DE ANÁLISE, ORÇ)
            // ================= CABEÇALHO =================
            PdfPTable topo = new PdfPTable(3);
            topo.WidthPercentage = 100;
            topo.SetWidths(new float[] { 23, 47, 20 });

            var pathimg = Path.Combine(_env.WebRootPath, "images", "astec.png");


            Image logo = Image.GetInstance(pathimg);
            logo.ScaleToFit(120, 45);

            topo.AddCell(new PdfPCell(logo)
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = 5,
                BorderColorRight = BaseColor.RED,
                BorderWidthRight = 0.8f
            });

            topo.AddCell(new PdfPCell(new Phrase("RELATÓRIO DE ANÁLISE", tituloFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });

            topo.AddCell(new PdfPCell(new Phrase($"Orç. {orcamentoDados.BudgetFK.ID}", bold))
            {
                Border = Rectangle.BOX,
                BorderColor = cinzaBorda,
                FixedHeight = 30,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });

            doc.Add(topo);

            #endregion

            // ===== LINHA SEPARADORA =====
            PdfPTable linha = new PdfPTable(1);
            linha.WidthPercentage = 100;
            linha.AddCell(new PdfPCell { Border = Rectangle.BOTTOM_BORDER, BorderColor = cinzaBorda, FixedHeight = 10 });

            doc.Add(linha);

            #region PRIMEIRA TABELA COM DADOS DO EQUIPAMENTO
            // ================= DADOS =================
            AddHeader(doc, "DADOS DO EQUIPAMENTO", header, cinzaHeader);

            PdfPTable dados = new PdfPTable(4);
            dados.WidthPercentage = 100;
            dados.SetWidths(new float[] { 25, 35, 27, 35 });

            AddRow(dados, "Modelo/Produto:", orcamentoDados.EquipamentFK.Equipment, "Data de fabricação:", orcamentoDados.EquipamentFK.Manufacturing_Date.ToString(), bold, normal);
            AddRow(dados, "Código de produto:", orcamentoDados.EquipamentFK.Product.ToString(), "Opcionais/Acessórios:", opt, bold, normal);
            AddRow(dados, "Número de série:", orcamentoDados.EquipamentFK.Serial.ToString(), "Data da análise:", orcamentoDados.BudgetFK.Date.ToString(), bold, normal);

            doc.Add(dados);

            #endregion

            #region TABELA DA DESCRIÇÃO DOS DEFEITOS
            // ================= DEFEITOS =================
            AddHeader(doc, "DESCRIÇÃO DOS DEFEITOS", header, cinzaHeader);

            PdfPTable defeitos1 = new PdfPTable(2);
            defeitos1.WidthPercentage = 100;
            defeitos1.SetWidths(new float[] { 30, 70 });

            defeitos1.AddCell(LabelCell("Defeito(s) encontrado(s):", bold));
            defeitos1.AddCell(ValueCell(
                orcamentoDados.ReviewFK.Defect_Reported,
                normal));

            doc.Add(defeitos1);

            #endregion

            #region TABELA DE FALHAS REGISTRADAS NA MEMÓRIA E DEFEITO INFORMADO PELO CLIENTE
            PdfPTable defeitos3 = new PdfPTable(2);
            defeitos3.WidthPercentage = 100;
            defeitos3.SetWidths(new float[] { 30, 70 });

            defeitos3.AddCell(LabelCell("Falhas registradas na memória:", bold));
            defeitos3.AddCell(ValueCell(orcamentoDados.ReviewFK.Logged_Faults, normal, true));

            defeitos3.AddCell(LabelCell("Defeito informado pelo cliente:", bold));
            defeitos3.AddCell(ValueCell(orcamentoDados.Problem, normal, true));

            doc.Add(defeitos3);

            #endregion

            #region TABELA TIPO DE DEFEITO COM 3 CHECKBOX APENAS (CONSTANTE, INTERMITENTE, SEM DEFEITO)
            // ===== TIPO DE DEFEITO (2 COLUNAS) =====
            PdfPTable defeitos2 = new PdfPTable(2);
            defeitos2.WidthPercentage = 100;
            defeitos2.SetWidths(new float[] { 30, 70 });

            PdfPCell tipo = LabelCell("Tipo de defeito:", bold);
            tipo.Rowspan = 3;
            tipo.VerticalAlignment = Element.ALIGN_MIDDLE;
            defeitos2.AddCell(tipo);
            switch (orcamentoDados.ReviewFK.Type_Defect)
            {
                case 0:
                    defeitos2.AddCell(CelulaOpcaoDefeito("Constante", true, normal));
                    defeitos2.AddCell(CelulaOpcaoDefeito("Intermitente", false, normal));
                    defeitos2.AddCell(CelulaOpcaoDefeito("Sem defeito", false, normal));
                    break;
                case 1:
                    defeitos2.AddCell(CelulaOpcaoDefeito("Constante", false, normal));
                    defeitos2.AddCell(CelulaOpcaoDefeito("Intermitente", true, normal));
                    defeitos2.AddCell(CelulaOpcaoDefeito("Sem defeito", false, normal));
                    break;
                case 2:
                    defeitos2.AddCell(CelulaOpcaoDefeito("Constante", false, normal));
                    defeitos2.AddCell(CelulaOpcaoDefeito("Intermitente", false, normal));
                    defeitos2.AddCell(CelulaOpcaoDefeito("Sem defeito", true, normal));
                    break;
                default:
                    defeitos2.AddCell(CelulaOpcaoDefeito("Constante", false, normal));
                    defeitos2.AddCell(CelulaOpcaoDefeito("Intermitente", false, normal));
                    defeitos2.AddCell(CelulaOpcaoDefeito("Sem defeito", false, normal));
                    break;
            }

            defeitos2.AddCell(LabelCell("Possíveis causas do(s) defeito(s):", bold));

            var defects = await _context.Possible_Defects.AsNoTracking().ToListAsync();
            int linhas = defects.Count / 2;
            PdfPTable t = new PdfPTable(2) { WidthPercentage = 100 };
            for (int i = 0; i <= linhas+1; i++)
            {
                var search1 = orcamentoDados.reviewDefectsFK.FirstOrDefault(e => e.Defects == defects[i].ID);
                if (search1 != null)
                    t.AddCell(i < defects.Count ? CelulaOpcaoDefeito(defects[i].Name, true, normal) : CelulaVazia());
                else
                    t.AddCell(i < defects.Count ? LinhaCausa(defects[i].Name, normal) : CelulaVazia());


                var search2 = orcamentoDados.reviewDefectsFK.FirstOrDefault(e => e.Defects == defects[i + 1].ID);

                if (search2 != null)
                    t.AddCell(i < defects.Count ? CelulaOpcaoDefeito(defects[i + 1].Name, true, normal) : CelulaVazia());
                else
                    t.AddCell(i < defects.Count ? LinhaCausa(defects[i + 1].Name, normal) : CelulaVazia());
                i++;
            }

            defeitos2.AddCell(new PdfPCell(t)
            {
                Padding = 6,
                BackgroundColor = cinzaClaro,
            });
            doc.Add(defeitos2);

            #endregion

            #region TABELA DE ATIVIDADES DE REPARO A SEREM EXECUTADAS
            // ================= ATIVIDADES =================
            AddHeader(doc, "ATIVIDADES DE REPARO A SEREM EXECUTADAS", header, cinzaHeader);

            var activities = await _context.Repair_Activities.AsNoTracking().ToListAsync();

            PdfPTable atividades = new PdfPTable(1) { WidthPercentage = 100 };
            foreach (var activite in orcamentoDados.ReviewActivitiesFK)
            {
                var act = activities.FirstOrDefault(e => e.ID == activite.Activities);
                atividades.AddCell(ValueCell($"• {act.Name}", normal));
            }

            doc.Add(atividades);

            #endregion

            #region TABELA DE TÉCNICO RESPONSÁVEL E VALOR DO REPARO
            // ================= RODAPÉ =================
            PdfPTable rodape = new PdfPTable(2);
            rodape.WidthPercentage = 100;
            rodape.SetWidths(new float[] { 50, 50 });

            rodape.AddCell(LabelCell("Técnico Responsável:", bold));
            rodape.AddCell(ValueCell(orcamentoDados.TechnicalName, normal, true));

            rodape.AddCell(LabelCell("Valor do reparo:", bold));
            rodape.AddCell(ValueCell($"R$ {orcamentoDados.BudgetFK.Value}", normal, true));

            doc.Add(rodape);

            #endregion

            //==================Espaço=================
            Paragraph espaco1 = new Paragraph("\n\n\n\n");
            doc.Add(espaco1);

            #region TABELA OBSERVAÇÕES
            // ================= OBSERVAÇÕES =================
            AddHeader(doc, "OBSERVAÇÕES", tituloFont, cinzaHeader);

            PdfPTable obs = new PdfPTable(1) { WidthPercentage = 100 };
            obs.AddCell(new PdfPCell(new Phrase(
               "• O equipamento será enviado com a parametrização que chegou a Waldesa.\n\n" +
               "• O drive ficará disponível na Waldesa Automação por um período de 1 (um) mês aguardando o cliente se pronunciar, " +
               "caso contrário o equipamento será enviado de volta para o proprietário sem aviso prévio.\n\n" +
               "• Sugerimos a correta dimensionamento do drive e a reavaliação do local/ambiente de instalação, " +
               "a fim de garantir a máxima eficiência e desempenho do equipamento apresentado pelo manual do usuário, " +
               "tais como: ventilação; falta a terra; dispositivo de proteção adequado (disjuntor motor ou fusível ultrarrápido).", bold))
            {
                Padding = 8,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(obs);

            #endregion
            /*
            if (doc.PageNumber > 0)
                doc.NewPage();

            #region CABEÇALHO PÁGINA NOVA (RECEBIMENTO)
            // ================= CABEÇALHO PÁGINA NOVA (RECEBIMENTO) ================
            PdfPTable topo2 = new PdfPTable(2);
            topo2.WidthPercentage = 100;
            topo2.SetWidths(new float[] { 25, 74 });

            Image logo2 = Image.GetInstance(@"img\astec.jpeg");
            logo2.ScaleToFit(120, 45);

            topo2.AddCell(new PdfPCell(logo2)
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = 5,
                BorderColorRight = BaseColor.RED,
                BorderWidthRight = 0.8f
            });

            topo2.AddCell(new PdfPCell(new Phrase("     RELATÓRIO DE ANÁLISE", tituloFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });

            doc.Add(topo2);
            doc.Add(linha);

            #endregion

            #region CÉLULA "REGISTRO DE FOTOS" CENTRALIZADA E COM BORDA
            // ================= SUBTÍTULO =================
            PdfPTable subtitulo = new PdfPTable(1);
            subtitulo.WidthPercentage = 100;
            subtitulo.HorizontalAlignment = Element.ALIGN_CENTER;

            subtitulo.AddCell(new PdfPCell(new Phrase("REGISTRO DE FOTOS", bold))
            {
                Border = Rectangle.BOX,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 6
            });

            doc.Add(subtitulo);

            #endregion

            // ================= ESPAÇO =================
            doc.Add(new Paragraph("\n"));

            #region TAG PÁGINA RECEBIMENTO
            // ================= TAG RECEBIMENTO =================
            PdfPTable tag = new PdfPTable(1);
            tag.WidthPercentage = 25;
            tag.HorizontalAlignment = Element.ALIGN_LEFT;

            tag.AddCell(new PdfPCell(new Phrase("RECEBIMENTO", bold))
            {
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            });

            doc.Add(tag);
            #endregion

            #region FOTO DE EXEMPLO DAS PÁGINAS DE FOTOS
            // ================= FOTO =================
            doc.Add(new Paragraph("\n"));

            Image foto = Image.GetInstance(@"Example.jpg");
            foto.ScaleToFit(450, 600);
            foto.Alignment = Element.ALIGN_CENTER;

            PdfPTable fotoBox = new PdfPTable(1);
            fotoBox.WidthPercentage = 100;

            fotoBox.AddCell(new PdfPCell(foto)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            });

            doc.Add(fotoBox);

            #endregion

            #region RODAPÉ MENSAGEM ABAIXO DAS FOTOS
            // ================= RODAPÉ =================
            doc.Add(new Paragraph("\n\n"));

            Paragraph rodape2 = new Paragraph(
                "A conclusão quanto as possíveis causas foram baseadas na análise do produto em laboratório.",
                new Font(bfRegular, 8, Font.ITALIC)
            );
            rodape2.Alignment = Element.ALIGN_CENTER;

            doc.Add(rodape2);

            #endregion

            #region PÁGINA ETIQUETA

            doc.NewPage();

            // ================= CABEÇALHO PÁGINA NOVA (ETIQUETA) ================

            doc.Add(topo2);
            doc.Add(linha);

            // ================= ESPAÇO =================
            doc.Add(new Paragraph("\n"));

            // ================= TAG ETIQUETA =================
            PdfPTable tag2 = new PdfPTable(1);
            tag2.WidthPercentage = 25;
            tag2.HorizontalAlignment = Element.ALIGN_LEFT;

            tag2.AddCell(new PdfPCell(new Phrase("ETIQUETA", bold))
            {
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            });

            doc.Add(tag2);

            // ================= FOTO =================
            doc.Add(new Paragraph("\n"));

            doc.Add(fotoBox);

            // ================= RODAPÉ =================

            doc.Add(rodape2);

            #endregion

            #region PÁGINA DESMONTAGEM E EVIDÊNCIA DE DANO
            doc.NewPage();

            // ================= CABEÇALHO PÁGINA NOVA (DESMONTAGEM E EVIDÊNCIA DE DANO) ================

            doc.Add(topo2);
            doc.Add(linha);

            // ================= ESPAÇO =================
            doc.Add(new Paragraph("\n"));

            // ================= TAG ETIQUETA =================
            PdfPTable tag3 = new PdfPTable(1);
            tag3.WidthPercentage = 25;
            tag3.HorizontalAlignment = Element.ALIGN_LEFT;

            tag3.AddCell(new PdfPCell(new Phrase("DESMONTAGEM E EVIDÊNCIA DE DANO", bold))
            {
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            });

            doc.Add(tag3);

            // ================= FOTO =================
            doc.Add(new Paragraph("\n"));

            doc.Add(fotoBox);

            // ================= RODAPÉ =================

            doc.Add(rodape2);

            #endregion

            #region PÁGINA SUJIDADE INTERNA
            doc.NewPage();

            // ================= CABEÇALHO PÁGINA NOVA (SUJIDADE INTERNA) ================

            doc.Add(topo2);
            doc.Add(linha);

            // ================= ESPAÇO =================
            doc.Add(new Paragraph("\n"));

            // ================= TAG ETIQUETA =================
            PdfPTable tag4 = new PdfPTable(1);
            tag4.WidthPercentage = 25;
            tag4.HorizontalAlignment = Element.ALIGN_LEFT;

            tag4.AddCell(new PdfPCell(new Phrase("SUJIDADE INTERNA", bold))
            {
                BackgroundColor = BaseColor.WHITE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            });

            doc.Add(tag4);

            // ================= FOTO =================
            doc.Add(new Paragraph("\n"));

            doc.Add(fotoBox);

            // ================= RODAPÉ =================

            doc.Add(rodape2);



        #endregion
            */

            doc.Close();


            return $"{filename}";
        }


        #region TUDO RELACIONADO AO CHECK BOX DA PRIMEIRA PÁGINA (TIPO DE DEFEITO E CAUSAS) COLUNA DIVIDIDA

        // ================= HELPERS =================

        static void AddHeader(Document doc, string text, Font font, BaseColor bg)
        {
            PdfPTable t = new PdfPTable(1) { WidthPercentage = 100 };
            t.AddCell(new PdfPCell(new Phrase(text, font))
            {
                BackgroundColor = bg,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 6
            });
            doc.Add(t);
        }

        static void AddRow(PdfPTable t, string l1, string v1, string l2, string v2, Font bold, Font normal)
        {
            t.AddCell(LabelCell(l1, bold));
            t.AddCell(ValueCell(v1, normal));
            t.AddCell(LabelCell(l2, bold));
            t.AddCell(ValueCell(v2, normal));
        }

        static PdfPCell LabelCell(string text, Font font) =>
            new PdfPCell(new Phrase(text, font))
            {
                Padding = 4,
                BackgroundColor = new BaseColor(245, 245, 245)
            };

        static PdfPCell ValueCell(string text, Font font, bool italico = false)
        {
            Font f = italico ? new Font(font.BaseFont, font.Size, Font.ITALIC) : font;
            return new PdfPCell(new Phrase(text, f)) { Padding = 4 };
        }

        // ===== CHECKBOX ESTÁTICO =====

        static PdfPCell CheckCell(bool marcado)
        {
            PdfPCell cell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                MinimumHeight = 14,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            cell.CellEvent = new StaticCheckboxCellEvent(marcado);
            return cell;
        }

        class StaticCheckboxCellEvent : IPdfPCellEvent
        {
            private readonly bool marcado;
            public StaticCheckboxCellEvent(bool marcado) => this.marcado = marcado;

            public void CellLayout(PdfPCell cell, Rectangle pos, PdfContentByte[] canvases)
            {
                PdfContentByte cb = canvases[PdfPTable.LINECANVAS];
                float size = 9;
                float x = pos.Left + (pos.Width - size) / 2;
                float y = pos.Bottom + (pos.Height - size) / 2;

                cb.Rectangle(x, y, size, size);
                cb.Stroke();

                if (marcado)
                {
                    cb.MoveTo(x + 1, y + 1);
                    cb.LineTo(x + size - 1, y + size - 1);
                    cb.MoveTo(x + 1, y + size - 1);
                    cb.LineTo(x + size - 1, y + 1);
                    cb.Stroke();
                }
            }
        }

        static PdfPCell CelulaOpcaoDefeito(string texto, bool marcado, Font normal)
        {
            PdfPTable t = new PdfPTable(2);
            t.SetWidths(new float[] { 8, 92 });
            t.WidthPercentage = 100;

            t.AddCell(CheckCell(marcado));
            t.AddCell(new PdfPCell(new Phrase(texto, normal))
            {
                Border = Rectangle.NO_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });

            return new PdfPCell(t) { Border = Rectangle.NO_BORDER };
        }


        static PdfPCell LinhaCausa(string texto, Font normal)
        {
            PdfPTable t = new PdfPTable(2);
            t.SetWidths(new float[] { 10, 90 });
            t.AddCell(CheckCell(false));
            t.AddCell(new PdfPCell(new Phrase(texto, normal)) { Border = Rectangle.NO_BORDER });
            return new PdfPCell(t) { Border = Rectangle.NO_BORDER };
        }

        static PdfPCell CelulaVazia() =>
            new PdfPCell { Border = Rectangle.NO_BORDER };
    }

    #endregion
}

