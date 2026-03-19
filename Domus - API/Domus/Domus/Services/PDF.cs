using Domus.Models.DB;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using Rectangle = iTextSharp.text.Rectangle;

namespace Domus.Services
{
    public class PDF
    {
        public static void pedidocompra(Purchase_Order order,int tenantID)
        {
            string pasta = @"PDF";

            if (!Directory.Exists(pasta))
            {
                Directory.CreateDirectory(pasta);
            }

            string newFile = Path.Combine(pasta, $"C-{order.ID}-{tenantID}.pdf");

            Document doc = new Document();
            PdfWriter write = PdfWriter.GetInstance(doc, new FileStream(newFile, FileMode.Create));

            doc.Open();

            PdfContentByte cb = write.DirectContentUnder;

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(bf, 20);

            #region Logo  Waldesa
            /*
            var imagem = iTextSharp.text.Image.GetInstance(@"imagens\Logo.png");
            imagem.ScalePercent(25f);
            imagem.SetAbsolutePosition(doc.LeftMargin, doc.PageSize.Height - 60);

            doc.Add(imagem);
            */
            #endregion

            #region Espaço entre o titulo e a logo

            Paragraph espaco1 = new Paragraph("\n");
            doc.Add(espaco1);

            #endregion



            #region titulo e espaço do titulo

            Paragraph titulo = new Paragraph(new Phrase($" Pedido de Compra - Pedido Nº {order.ID}", new Font(bf, 26f, Font.BOLD, BaseColor.BLACK)));
            titulo.Alignment = Element.ALIGN_CENTER;

            Paragraph espaço = new Paragraph("\n\n");

            doc.Add(titulo);
            doc.Add(espaço);

            #endregion

            #region informações do pedido

            PdfPTable info = new PdfPTable(2);
            info.WidthPercentage = 100;
            info.SetWidths(new float[] { 18f, 68f });

            Font labelFont = new Font(bf, 10f, Font.BOLD, BaseColor.WHITE);
            Font valueFont = new Font(bf, 10f, Font.NORMAL, BaseColor.BLACK);

            var unit = "";

            switch (tenantID)
            {
                case 1:
                    unit = "Waldesa Comercio";
                        break;
                case 2:
                    unit = "Waldesa MotoMercantil (Jundiapeba)";
                        break;
                case 3:
                    unit = "Waldesa MotoMercantil (Braz Cubas)";
                        break;
                case 4:
                    unit = "Waldesa Motomercantil RJ";
                        break;
            }

            string[,] dados = {
{ "Número do Pedido:", $"{order.ID}" },
{ "Solicitante:", $"{order.RequestFK.RequesterFK.Name}" },
{ "Unidade:", $"{unit}" },
{ "Departamento:", $"{order.RequestFK.DepartmentFK.Name}" },
{ "Uso do Material:", $"{order.RequestFK.UseFK.Name}" },
{ "Recomendação de Fornecedor:", $"{order.SupplierFK.Name}" }
        };

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                BaseColor bgcolor = (i % 2 == 0) ? new BaseColor(240, 240, 240) : BaseColor.WHITE;

                PdfPCell label = new PdfPCell(new Phrase(dados[i, 0], labelFont));
                label.BackgroundColor = new BaseColor(49, 102, 173);
                label.Border = Rectangle.BOTTOM_BORDER;
                label.HorizontalAlignment = Element.ALIGN_CENTER;
                label.BorderColor = BaseColor.WHITE;
                label.Padding = 5;

                PdfPCell value = new PdfPCell(new Phrase(dados[i, 1], valueFont));
                value.BackgroundColor = bgcolor;
                value.Border = Rectangle.NO_BORDER;
                value.Padding = 5;
                info.AddCell(label);
                info.AddCell(value);
            }

            doc.Add(info);
            doc.Add(new Paragraph("\n"));

            #endregion

            #region tabela de produtos

            PdfPTable tabela = new PdfPTable(5);
            tabela.WidthPercentage = 100;
            tabela.SetWidths(new float[] { 10f, 30f, 10f, 15f, 15f });
            tabela.HeaderRows = 1;

            Font headerFont = new Font(bf, 9f, Font.BOLD, BaseColor.WHITE);
            Font cellFont = new Font(bf, 9f, Font.NORMAL, BaseColor.BLACK);

            #region Header
            string[] headers = {
            "Código", "Descrição do Produto", "Qtde",
            "Valor Unitário Aproximado", "Valor Total"
        };

            foreach (var h in headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(h, headerFont));
                cell.BackgroundColor = new BaseColor(49, 102, 173);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = Rectangle.NO_BORDER;
                cell.Padding = 6;
                tabela.AddCell(cell);
                #endregion


            }

            #region Gerador de Produtos
            List<string[]> produtosList = new List<string[]>();

            foreach (var item in order.RequestFK.ItemsFK)
            {
                string codigo = $"{item.Product}";
                string descricao = item.ProductFK.Description;
                string quantidade = $"{item.Amount}";
                Decimal total = item.Unit_Value * int.Parse(quantidade);
                string valorUnitario = $"R$ {item.Unit_Value:0.00}";
                string valorTotal = $"R$ {total:0.00}";

                produtosList.Add(new string[] { codigo, descricao, quantidade, valorUnitario, valorTotal });
            }
            #endregion


            for (int i = 0; i < produtosList.Count; i++)
            {
                var produto = produtosList[i];
                BaseColor bgColor = (i % 2 == 0) ? new BaseColor(230, 230, 230) : BaseColor.WHITE;

                for (int j = 0; j < produto.Length; j++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(produto[j], cellFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 5;
                    cell.BackgroundColor = bgColor;
                    cell.Border = Rectangle.NO_BORDER;
                    tabela.AddCell(cell);
                }
            }

            doc.Add(tabela);
            doc.Add(new Paragraph("\n"));

            #endregion

            #region rodapé

            Paragraph rodape = new Paragraph($" Emitido em: {DateTime.Now.ToString("D", new CultureInfo("pt-BR"))}\n Documento gerado automaticamente. Válido para fins internos.",
                new Font(bf, 9f, Font.ITALIC, BaseColor.BLACK));
            rodape.Alignment = Element.ALIGN_RIGHT;

            doc.Add(rodape);

            #endregion



            doc.Close();




        }
        public static void chamado(Service_Order order, int tenantID)
        {
            string pasta = @"PDF";

            if (!Directory.Exists(pasta))
            {
                Directory.CreateDirectory(pasta);
            }

            string newFile = Path.Combine(pasta,$"{order.ID}-{tenantID}.pdf");

            Document doc = new Document();
            PdfWriter write = PdfWriter.GetInstance(doc, new FileStream(newFile, FileMode.Create));

            doc.Open();

            PdfContentByte cb = write.DirectContentUnder;

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(bf, 20);
            cb.Stroke();


            #region Titulo1 - Logo e Registro do Chamado N°
            /*
            var imagem = iTextSharp.text.Image.GetInstance(@"imagens\Logo.png");
            imagem.ScalePercent(25f);
            imagem.SetAbsolutePosition(doc.LeftMargin, doc.PageSize.Height - 60);
            */
            Paragraph titulo = new Paragraph(new Phrase($"Registro do Chamado - N° {order.ID}", new Font(bf, 26f, Font.BOLD, BaseColor.BLACK)));
            titulo.Alignment = Element.ALIGN_CENTER;


            //doc.Add(imagem);
            doc.Add(titulo);

            #endregion



            #region informações (numero do chamado, id desktop, quem solicitou, Problema relatado,data do chamado, tecnico que atendeu,data e hora do contato)

            Paragraph space = new Paragraph("\n");

            doc.Add(space);


            PdfPTable info = new PdfPTable(2);
            info.WidthPercentage = 100;
            info.SetWidths(new float[] { 19f, 55f });

            Font leftinfo = new Font(bf, 10f, Font.BOLD, BaseColor.WHITE);
            Font rightinfo = new Font(bf, 10f, Font.NORMAL, BaseColor.BLACK);


            string[,] infodados = {
         {"Registro da Solicitação:", $"{order.Requested_Date}" },
         {"Número do Chamado:", $"{order.ID}" },
         {"Identificador da Máquina:", $"{order.Computer}" },
         {"Solicitante:", $"{order.RequestFK.Name}" },
         {"Problema Relatado:", $"{order.Problem}" },
         {"Técnico Responsável:", $"{order.TechnicalFK.Name}" },
         {"Momento do Contato:", $"{order.Contact_Date}" }
     };

            for (int i = 0; i < infodados.GetLength(0); i++)
            {
                BaseColor bgdados = (i % 2 == 0) ? new BaseColor(240, 240, 240) : BaseColor.WHITE;

                PdfPCell leftdados = new PdfPCell(new Phrase(infodados[i, 0], leftinfo));
                leftdados.BackgroundColor = new BaseColor(49, 102, 173);
                leftdados.Border = Rectangle.BOTTOM_BORDER;
                leftdados.HorizontalAlignment = Element.ALIGN_LEFT;
                leftdados.BorderColor = BaseColor.WHITE;
                leftdados.Padding = 5;

                PdfPCell rightdados = new PdfPCell(new Phrase(infodados[i, 1], rightinfo));
                rightdados.HorizontalAlignment = Element.ALIGN_LEFT;
                rightdados.BackgroundColor = bgdados;
                rightdados.Border = Rectangle.NO_BORDER;
                rightdados.Padding = 5;
                info.AddCell(leftdados);
                info.AddCell(rightdados);




            }

            doc.Add(info);

            #endregion


            #region Titulo2 - Procedimento Realizado no Chamado

            doc.Add(space);

            Paragraph titulo2 = new Paragraph(new Phrase("- Lista de Materiais Usados no Chamado -", new Font(bf, 18f, Font.BOLD, BaseColor.BLACK)));
            titulo2.Alignment = Element.ALIGN_CENTER;

            doc.Add(titulo2);

            #endregion


            #region Tabela de Procedimentos Realizados

            doc.Add(space);

            //Primeiro cabeçalho (Materiais Utilizados na Solicitação)

            PdfPTable list = new PdfPTable(1);
            list.WidthPercentage = 100;
            list.SetWidths(new float[] { 100f });

            Font leftlist = new Font(bf, 13f, Font.BOLD, BaseColor.WHITE);


            string[,] listdados = {
         {"Materiais Utilizados no Chamado:" }
     };

            for (int i = 0; i < listdados.GetLength(0); i++)
            {
                BaseColor bglist = (i % 2 == 0) ? new BaseColor(240, 240, 240) : BaseColor.WHITE;

                PdfPCell headerlist = new PdfPCell(new Phrase(listdados[i, 0], leftlist));
                headerlist.BackgroundColor = new BaseColor(49, 102, 173);
                headerlist.Border = Rectangle.BOTTOM_BORDER;
                headerlist.BorderColor = BaseColor.WHITE;
                headerlist.HorizontalAlignment = Element.ALIGN_CENTER;
                headerlist.Padding = 5;


                //Segundo cabeçalho (Descrição do material, Quantidade)

                PdfPTable head = new PdfPTable(2);
                head.WidthPercentage = 100;
                head.SetWidths(new float[] { 68f, 20f });

                Font headfont = new Font(bf, 11f, Font.BOLD, BaseColor.WHITE);


                string[,] header = {
         {"Descrição do Material Utilizado", "Quantidade" }
     };

                {
                    BaseColor bgheader = (i % 2 == 0) ? new BaseColor(240, 240, 240) : BaseColor.WHITE;

                    PdfPCell quanti = new PdfPCell(new Phrase(header[i, 0], headfont));
                    quanti.BackgroundColor = new BaseColor(49, 102, 173);
                    quanti.Border = Rectangle.NO_BORDER;
                    quanti.BorderColor = BaseColor.WHITE;
                    quanti.HorizontalAlignment = Element.ALIGN_CENTER;
                    quanti.Padding = 5;

                    PdfPCell desc = new PdfPCell(new Phrase(header[i, 1], headfont));
                    desc.BackgroundColor = new BaseColor(49, 102, 173);
                    desc.Border = Rectangle.LEFT_BORDER;
                    desc.BorderColor = BaseColor.WHITE;
                    desc.HorizontalAlignment = Element.ALIGN_CENTER;
                    desc.Padding = 5;


                    list.AddCell(headerlist);
                    head.AddCell(quanti);
                    head.AddCell(desc);


                }

                doc.Add(list);
                doc.Add(head);

                //Lista dos materiais e quantidade

                PdfPTable resultli = new PdfPTable(2);
                resultli.WidthPercentage = 100;
                resultli.SetWidths(new float[] { 68f, 20f });

                Font refont = new Font(bf, 10f, Font.NORMAL, BaseColor.BLACK);

                int count = 0;

                foreach (var item in order.Service_ItemsFK)
                {
                    BaseColor bgreslist = (count % 2 == 0) ? new BaseColor(240, 240, 240) : BaseColor.WHITE;

                    PdfPCell reslistleft = new PdfPCell(new Phrase(item.ProductFK.Description, refont));
                    reslistleft.BackgroundColor = bgreslist;
                    reslistleft.Border = Rectangle.NO_BORDER;
                    reslistleft.HorizontalAlignment = Element.ALIGN_LEFT;
                    reslistleft.Padding = 10;

                    PdfPCell reslistright = new PdfPCell(new Phrase($"{item.Amount} {item.ProductFK.ProductsFK.Measure}", refont));
                    reslistright.BackgroundColor = bgreslist;
                    reslistright.Border = Rectangle.LEFT_BORDER;
                    reslistright.BorderColor = new BaseColor(204, 204, 204);
                    reslistright.HorizontalAlignment = Element.ALIGN_CENTER;
                    reslistright.Padding = 10;

                    resultli.AddCell(reslistleft);
                    resultli.AddCell(reslistright);
                    count++;

                }


                doc.Add(resultli);


                #endregion


                #region Titulo 3 Processo da Solução do Problema

                doc.Add(space);

                Paragraph titulo3 = new Paragraph(new Phrase("- Processo Realizado para Solução do Problema -", new Font(bf, 16f, Font.BOLD, BaseColor.BLACK)));
                titulo3.Alignment = Element.ALIGN_CENTER;

                doc.Add(titulo3);

                #endregion


                #region tabela do proceso realizado para solução do problema

                doc.Add(space);

                PdfPTable procediment = new PdfPTable(1);
                procediment.WidthPercentage = 100;
                procediment.SetWidths(new float[] { 100f });

                Font prochead = new Font(bf, 13f, Font.BOLD, BaseColor.WHITE);


                string[,] procfloat = {
         {"Serviços Realizados" },
     };

                for (int u = 0; u < procfloat.GetLength(0); u++)
                {
                    BaseColor bgproc = (u % 2 == 0) ? new BaseColor(240, 240, 240) : BaseColor.WHITE;

                    PdfPCell procwrited = new PdfPCell(new Phrase(procfloat[u, 0], prochead));
                    procwrited.BackgroundColor = new BaseColor(49, 102, 173);
                    procwrited.Border = Rectangle.BOTTOM_BORDER;
                    procwrited.BorderColor = BaseColor.WHITE;
                    procwrited.HorizontalAlignment = Element.ALIGN_CENTER;
                    procwrited.Padding = 5;

                    procediment.AddCell(procwrited);


                    PdfPTable resultproc = new PdfPTable(1);
                    resultproc.WidthPercentage = 100;
                    resultproc.SetWidths(new float[] { 100f });

                    Font procwrite = new Font(bf, 10f, Font.NORMAL, BaseColor.BLACK);

                    foreach(var item in order.Service_ExecuteFK)
                    {

                        PdfPCell procresult = new PdfPCell(new Phrase(item.ServiceFK.Name, procwrite));
                        procresult.BackgroundColor = bgproc;
                        procresult.Border = Rectangle.BOTTOM_BORDER;
                        procresult.BorderColor = BaseColor.WHITE;
                        procresult.HorizontalAlignment = Element.ALIGN_LEFT;
                        procresult.Padding = 7;


                        resultproc.AddCell(procresult);


                    }

                    doc.Add(procediment);
                    doc.Add(resultproc);
                }
                #region Titulo4 - Check-List Realizado no Chamado

                doc.Add(space);

                Paragraph titulo4 = new Paragraph(new Phrase("- Lista de Check-List no Chamado -", new Font(bf, 18f, Font.BOLD, BaseColor.BLACK)));
                titulo2.Alignment = Element.ALIGN_CENTER;

                doc.Add(titulo4);

                #endregion


                #region Tabela de Procedimentos Realizados

                doc.Add(space);

                    head = new PdfPTable(2);
                    head.WidthPercentage = 100;
                    head.SetWidths(new float[] { 68f, 20f });

                    string[,] header1 = {
         {"Propriedade", "Avaliação" }
     };

                    {
                        BaseColor bgheader = (i % 2 == 0) ? new BaseColor(240, 240, 240) : BaseColor.WHITE;

                        PdfPCell quanti = new PdfPCell(new Phrase(header1[i, 0], headfont));
                        quanti.BackgroundColor = new BaseColor(49, 102, 173);
                        quanti.Border = Rectangle.NO_BORDER;
                        quanti.BorderColor = BaseColor.WHITE;
                        quanti.HorizontalAlignment = Element.ALIGN_CENTER;
                        quanti.Padding = 5;

                        PdfPCell desc = new PdfPCell(new Phrase(header1[i, 1], headfont));
                        desc.BackgroundColor = new BaseColor(49, 102, 173);
                        desc.Border = Rectangle.LEFT_BORDER;
                        desc.BorderColor = BaseColor.WHITE;
                        desc.HorizontalAlignment = Element.ALIGN_CENTER;
                        desc.Padding = 5;


                        list.AddCell(headerlist);
                        head.AddCell(quanti);
                        head.AddCell(desc);


                    }

                    doc.Add(head);

                    //Lista dos materiais e quantidade

                    resultli = new PdfPTable(2);
                    resultli.WidthPercentage = 100;
                    resultli.SetWidths(new float[] { 68f, 20f });

                    count = 0;

                    foreach (var item in order.Service_CheckListFK)
                    {
                        BaseColor bgreslist = (count % 2 == 0) ? new BaseColor(240, 240, 240) : BaseColor.WHITE;

                        PdfPCell reslistleft = new PdfPCell(new Phrase(item.CheckListFK.Check, refont));
                        reslistleft.BackgroundColor = bgreslist;
                        reslistleft.Border = Rectangle.NO_BORDER;
                        reslistleft.HorizontalAlignment = Element.ALIGN_LEFT;
                        reslistleft.Padding = 10;

                        PdfPCell reslistright = new PdfPCell(new Phrase(item.Note, refont));
                        reslistright.BackgroundColor = bgreslist;
                        reslistright.Border = Rectangle.LEFT_BORDER;
                        reslistright.BorderColor = new BaseColor(204, 204, 204);
                        reslistright.HorizontalAlignment = Element.ALIGN_CENTER;
                        reslistright.Padding = 10;

                        resultli.AddCell(reslistleft);
                        resultli.AddCell(reslistright);
                        count++;

                    }


                    doc.Add(resultli);


                    #endregion

                    #endregion


                    #region marca dágua da data

                    Paragraph rodape = new Paragraph(" Emitido em: 19 de setembro de 2025\n Documento gerado automaticamente. Válido para fins internos.",
                new Font(bf, 9f, Font.ITALIC, BaseColor.BLACK));
                rodape.Alignment = Element.ALIGN_RIGHT;

                doc.Add(rodape);


                #endregion


                #region linha de assinatura

                /*
                PdfPTable linha = new PdfPTable(1);
                linha.WidthPercentage = 100;
                linha.SetWidths(new[] { 100f });

                Font linha1 = new Font(bf, 9f, Font.BOLDITALIC, BaseColor.BLACK);

                PdfPCell linhaass = new PdfPCell(new Phrase("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n        ________________________________________________________.\n\n              Assinatura do técnico.", linha1));
                linhaass.Border = Rectangle.NO_BORDER;
                linhaass.HorizontalAlignment = Element.ALIGN_RIGHT;


                linha.AddCell(linhaass);
                doc.Add(linha);

                var assinatura = (byte[])order.TechnicalFK.Signature;

                var imagem = iTextSharp.text.Image.GetInstance(assinatura);
                imagem.ScalePercent(25f);
                imagem.SetAbsolutePosition(doc.RightMargin, doc.PageSize.Height - 60);
                doc.Add(imagem);
                */
                #endregion

                doc.Close();
            }
        }
    }
}
