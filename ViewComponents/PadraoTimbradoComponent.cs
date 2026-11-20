using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

public class PadraoTimbradoComponent : IComponent
{
    private readonly string _tituloPagina;
    private readonly string _numeroProcesso;
    private readonly byte[] _logoBytes;
    private readonly byte[] _marcaDaguaBytes;
    private readonly Action<IContainer> _conteudoPrincipal;

    // Dados institucionais fixos da empresa
    public static readonly string NomeEmpresa = "CredMaster Microcrédito E.I.";
    public static readonly string RodapeTimbrado = "Bairro Agostinho Neto   -   Av. Moçambique   -   Cell: +258 846979539 / 866979539   -   Maputo - Moçambique";

    public PadraoTimbradoComponent(
        string tituloPagina,
        string numeroProcesso,
        byte[] logoBytes,
        byte[] marcaDaguaBytes,
        Action<IContainer> conteudoPrincipal)
    {
        _tituloPagina = tituloPagina;
        _numeroProcesso = numeroProcesso;
        _logoBytes = logoBytes;
        _marcaDaguaBytes = marcaDaguaBytes;
        _conteudoPrincipal = conteudoPrincipal;
    }

    public void Compose(IContainer container)
    {
        var culturaPt = new System.Globalization.CultureInfo("pt-PT");

        container.Column(colunaGeral =>
        {
            colunaGeral.Spacing(8);

            // --- CABEÇALHO COM LOGOTIPO E INFO À DIREITA ---
            colunaGeral.Item().Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    if (_logoBytes != null)
                    {
                        c.Item().Width(110).Image(_logoBytes);
                    }
                    else
                    {
                        c.Item().Text("CredMaster").Bold().FontSize(18).FontColor("#079b62");
                        c.Item().Text(NomeEmpresa).FontSize(8f).FontColor("#718096");
                    }
                });

                row.ConstantItem(180).AlignRight().Column(c =>
                {
                    c.Item().Text($"Data: {DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", culturaPt)}").FontSize(8.5f).Italic().AlignRight();
                    if (!string.IsNullOrEmpty(_numeroProcesso))
                    {
                        c.Item().Text($"Ref: {_numeroProcesso}").Bold().FontSize(8.5f).FontColor("#079b62").AlignRight();
                    }
                });
            });

            colunaGeral.Item().PaddingVertical(3).LineHorizontal(1).LineColor("#079b62");

            // Título em Bloco Verde
            if (!string.IsNullOrEmpty(_tituloPagina))
            {
                colunaGeral.Item().Background("#079b62").Padding(6).AlignCenter().Text(_tituloPagina)
                    .Bold().FontSize(11).FontColor("#FFFFFF");

                colunaGeral.Item().PaddingTop(3);
            }

            // Injeta o conteúdo específico
            colunaGeral.Item().Element(element => _conteudoPrincipal(element));
        });
    }
}