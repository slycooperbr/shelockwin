using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;

namespace sherlockwin
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Azul predominante
            var azulEscuro = Color.FromArgb(20, 40, 80);
            var azulClaro = Color.FromArgb(100, 149, 237);
            var azulMedio = Color.FromArgb(30, 60, 120);
            var branco = Color.White;

            var form = new Form();
            form.Text = "SherlockWin";
            form.Width = 1220;
            form.Height = 1020;
            form.BackColor = azulEscuro;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.MinimizeBox = true;
            form.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            form.ShowIcon = false;

            var label = new Label()
            {
                Text = "Nome de usuário:",
                ForeColor = branco,
                Left = 20,
                Top = 18,
                Width = 140,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                BackColor = Color.Transparent
            };
            form.Controls.Add(label);

            var usernameBox = new TextBox()
            {
                Left = 170,
                Top = 15,
                Width = 350,
                Font = new Font("Segoe UI", 12F),
                BackColor = azulClaro,
                ForeColor = azulEscuro,
                BorderStyle = BorderStyle.FixedSingle
            };
            form.Controls.Add(usernameBox);

            var searchButton = new Button()
            {
                Text = "Buscar perfis",
                Left = 540,
                Top = 13,
                Width = 140,
                Height = 35,
                BackColor = azulMedio,
                ForeColor = branco,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };
            searchButton.FlatAppearance.BorderColor = azulClaro;
            form.Controls.Add(searchButton);

            var imgSearchButton = new Button()
            {
                Text = "Buscar imagens públicas",
                Left = 690,
                Top = 13,
                Width = 210,
                Height = 35,
                BackColor = azulMedio,
                ForeColor = branco,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };
            imgSearchButton.FlatAppearance.BorderColor = azulClaro;
            form.Controls.Add(imgSearchButton);

            var searchEnginesButton = new Button()
            {
                Text = "Buscar nos mecanismos",
                Left = 910,
                Top = 13,
                Width = 210,
                Height = 35,
                BackColor = azulMedio,
                ForeColor = branco,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };
            searchEnginesButton.FlatAppearance.BorderColor = azulClaro;
            form.Controls.Add(searchEnginesButton);

            var resultsBox = new TextBox()
            {
                Left = 20,
                Top = 60,
                Width = 1140,
                Height = 200,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Segoe UI", 10),
                BackColor = azulClaro,
                ForeColor = azulEscuro,
                BorderStyle = BorderStyle.FixedSingle
            };
            form.Controls.Add(resultsBox);

            // Label e painel UNIFICADO para imagens públicas de Bing+Google
            var labelImagens = new Label()
            {
                Text = "Resultados de Imagens Públicas (Bing e Google)",
                Left = 20,
                Top = 270,
                Width = 600,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = branco,
                BackColor = Color.Transparent
            };
            form.Controls.Add(labelImagens);

            var imagensPanel = new FlowLayoutPanel()
            {
                Left = 20,
                Top = 300,
                Width = 1140,
                Height = 390,
                AutoScroll = true,
                BackColor = azulClaro
            };
            form.Controls.Add(imagensPanel);

            var labelSearchEngines = new Label()
            {
                Text = "Resultados dos mecanismos de busca (Google e Bing)",
                Left = 20,
                Top = 700,
                Width = 700,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = branco,
                BackColor = Color.Transparent
            };
            form.Controls.Add(labelSearchEngines);

            var enginesBox = new TextBox()
            {
                Left = 20,
                Top = 730,
                Width = 1140,
                Height = 220,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Segoe UI", 10),
                BackColor = azulClaro,
                ForeColor = azulEscuro,
                BorderStyle = BorderStyle.FixedSingle
            };
            form.Controls.Add(enginesBox);

            var sites = new Dictionary<string, string>
            {
                {"GitHub", "https://github.com/{0}"},
                {"Reddit", "https://www.reddit.com/user/{0}"},
                {"Instagram", "https://instagram.com/{0}"},
                {"Facebook", "https://facebook.com/{0}"},
                {"Twitter", "https://twitter.com/{0}"},
                {"YouTube", "https://www.youtube.com/@{0}"}
            };

            searchButton.Click += async (sender, args) =>
            {
                string username = usernameBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Digite um nome de usuário.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                resultsBox.Text = "Procurando perfis...\r\n";

                using (HttpClient client = new HttpClient())
                {
                    foreach (var site in sites)
                    {
                        string siteName = site.Key;
                        string url = string.Format(site.Value, username);

                        bool found = false;
                        try
                        {
                            var resp = await client.GetAsync(url);
                            found = resp.IsSuccessStatusCode;
                        }
                        catch { }

                        if (found)
                        {
                            resultsBox.AppendText($"[+] {siteName}: Encontrado! {url}\r\n");
                        }
                        else
                        {
                            resultsBox.AppendText($"[-] {siteName}: Não encontrado.\r\n");
                        }
                    }
                }
                resultsBox.AppendText("Busca de perfis finalizada!\r\n");
            };

            imgSearchButton.Click += async (sender, args) =>
            {
                string searchQuery = usernameBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(searchQuery))
                {
                    MessageBox.Show("Digite um termo para buscar imagens.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                resultsBox.AppendText($"\r\nBuscando imagens públicas relacionadas a \"{searchQuery}\" no Bing e Google...\r\n");
                imagensPanel.Controls.Clear();

                resultsBox.AppendText("Buscando no Bing...\r\n");
                var bingImgs = await BuscarImagensBing(searchQuery, 10);
                resultsBox.AppendText($"Bing: {bingImgs.Count} imagens encontradas\r\n");

                resultsBox.AppendText("Buscando no Google...\r\n");
                var googleImgs = await BuscarImagensGoogle(searchQuery, 10);
                resultsBox.AppendText($"Google: {googleImgs.Count} imagens encontradas\r\n");

                // Mostra as imagens do Bing (com borda azul média)
                foreach (var url in bingImgs)
                {
                    try
                    {
                        var pic = new PictureBox();
                        pic.Width = pic.Height = 160;
                        pic.SizeMode = PictureBoxSizeMode.Zoom;
                        pic.LoadAsync(url);
                        pic.BorderStyle = BorderStyle.FixedSingle;
                        pic.BackColor = azulMedio;
                        imagensPanel.Controls.Add(pic);
                    }
                    catch { }
                }
                // Mostra as imagens do Google (com borda azul claro)
                foreach (var url in googleImgs)
                {
                    try
                    {
                        var pic = new PictureBox();
                        pic.Width = pic.Height = 160;
                        pic.SizeMode = PictureBoxSizeMode.Zoom;
                        pic.LoadAsync(url);
                        pic.BorderStyle = BorderStyle.FixedSingle;
                        pic.BackColor = azulClaro;
                        imagensPanel.Controls.Add(pic);
                    }
                    catch { }
                }

                resultsBox.AppendText("Busca finalizada!\r\n");
            };

            searchEnginesButton.Click += async (sender, args) =>
            {
                string termo = usernameBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(termo))
                {
                    MessageBox.Show("Digite um termo para buscar!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                enginesBox.Text = "Buscando nos mecanismos de busca...\r\n";

                var queries = new List<(string desc, string query)>
                {
                    ("Facebook", $"site:facebook.com {termo}"),
                    ("Instagram", $"inurl:instagram.com {termo}"),
                    ("Twitter", $"site:twitter.com {termo}"),
                    ("YouTube", $"site:youtube.com {termo}"),
                    ("Reddit", $"site:reddit.com {termo}"),
                    ("GitHub", $"site:github.com {termo}")
                };

                foreach (var (desc, query) in queries)
                {
                    enginesBox.AppendText($"\r\n---\r\n{desc} (Google):\r\n");
                    var googleLinks = await BuscarLinksGoogle(query, 3);
                    if (googleLinks.Count == 0) enginesBox.AppendText("Nenhum resultado encontrado.\r\n");
                    else foreach (var l in googleLinks) enginesBox.AppendText($"{l}\r\n");

                    enginesBox.AppendText($"{desc} (Bing):\r\n");
                    var bingLinks = await BuscarLinksBing(query, 3);
                    if (bingLinks.Count == 0) enginesBox.AppendText("Nenhum resultado encontrado.\r\n");
                    else foreach (var l in bingLinks) enginesBox.AppendText($"{l}\r\n");
                }
                enginesBox.AppendText("\r\nBusca nos mecanismos finalizada!\r\n");
            };

            Application.Run(form);
        }

        static async Task<List<string>> BuscarImagensBing(string termoBusca, int maxImagens)
        {
            var urls = new List<string>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    string url = $"https://www.bing.com/images/search?q={Uri.EscapeDataString(termoBusca)}&form=HDRSC2";
                    string html = await client.GetStringAsync(url);

                    var matches = Regex.Matches(html, @"murl&quot;:&quot;(https:\/\/[^&]+)&quot;");
                    foreach (Match match in matches)
                    {
                        if (match.Groups.Count > 1)
                        {
                            string imgUrl = match.Groups[1].Value;
                            if (!urls.Contains(imgUrl))
                                urls.Add(imgUrl);

                            if (urls.Count >= maxImagens)
                                break;
                        }
                    }
                }
            }
            catch { }
            return urls;
        }

        static async Task<List<string>> BuscarImagensGoogle(string termoBusca, int maxImagens)
        {
            var urls = new List<string>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    string url = $"https://www.google.com/search?tbm=isch&q={Uri.EscapeDataString(termoBusca)}";
                    string html = await client.GetStringAsync(url);

                    var matches = Regex.Matches(html, @"https://[^\""\\]+\.jpg");
                    foreach (Match match in matches)
                    {
                        string imgUrl = match.Value;
                        if (!urls.Contains(imgUrl) && !imgUrl.Contains("gstatic.com/"))
                        {
                            urls.Add(imgUrl);
                            if (urls.Count >= maxImagens)
                                break;
                        }
                    }
                }
            }
            catch { }
            return urls;
        }

        static async Task<List<string>> BuscarLinksGoogle(string query, int maxResults)
        {
            var links = new List<string>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    string url = $"https://www.google.com/search?q={Uri.EscapeDataString(query)}";
                    string html = await client.GetStringAsync(url);

                    var matches = Regex.Matches(html, @"<a href=""/url\?q=([^&""]+)");
                    foreach (Match match in matches)
                    {
                        if (match.Groups.Count > 1)
                        {
                            string link = System.Web.HttpUtility.UrlDecode(match.Groups[1].Value);
                            if (!link.Contains("google.com") && !link.Contains("webcache.googleusercontent.com") && !links.Contains(link))
                            {
                                links.Add(link);
                                if (links.Count >= maxResults)
                                    break;
                            }
                        }
                    }
                }
            }
            catch { }
            return links;
        }

        static async Task<List<string>> BuscarLinksBing(string query, int maxResults)
        {
            var links = new List<string>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    string url = $"https://www.bing.com/search?q={Uri.EscapeDataString(query)}";
                    string html = await client.GetStringAsync(url);

                    var matches = Regex.Matches(html, @"<li class=""b_algo"">.*?<a href=""([^""]+)""");
                    foreach (Match match in matches)
                    {
                        if (match.Groups.Count > 1)
                        {
                            string link = match.Groups[1].Value;
                            if (!links.Contains(link))
                            {
                                links.Add(link);
                                if (links.Count >= maxResults)
                                    break;
                            }
                        }
                    }
                }
            }
            catch { }
            return links;
        }
    }
}