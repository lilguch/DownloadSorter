using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadsSorter
{
    public partial class MainForm : Form
    {
        private readonly Color DefaultButtonColor = ColorTranslator.FromHtml("#7ec999");

        private const float HoverDarkenPercent = 0.15f;

        public MainForm()
        {
            InitializeComponent();
            InitializeOrganizer();
            SetupRulesPanel();
            LoadRules();
            DisplayRules();
            UpdateButtonState(StandartSorterStart_button);
            this.Load += (s, e) => RoundAllButtonsRecursive(this,5);
        }

        private DownloadOrganizer _organizer;
        private List<SorterRule> _rules;
        private Panel _rulesPanel;

        private void InitializeOrganizer()
        {
            if (Program._organizer != null)
            {
                _organizer = Program._organizer;
            }
            else
            {
                _organizer = new DownloadOrganizer(Program._notifyIcon);
                Program._organizer = _organizer;
            }
        }

        private void RoundAllButtonsRecursive(Control parent, int radius)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Button button)
                {
                    ApplyRoundedStyle(button, radius);
                }
            }
        }

        private void ApplyRoundedStyle(Button button, int radius)
        {
            if (button.Name == "StandartSorterStart_button")
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.ForeColor = Color.White;

                button.Resize -= Button_Resize;
                button.Resize += Button_Resize;

                SetRoundedRegion(button, radius);
                return;
            }

            button.MouseEnter -= Button_MouseEnter;
            button.MouseLeave -= Button_MouseLeave;
            button.Resize -= Button_Resize;

            if (!(button.Tag is Color))
                button.Tag = DefaultButtonColor;
            button.BackColor = DefaultButtonColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.ForeColor = Color.White;

            SetRoundedRegion(button, radius);

            button.MouseEnter += Button_MouseEnter;
            button.MouseLeave += Button_MouseLeave;
            button.Resize += Button_Resize;
        }

        private void SetRoundedRegion(Button button, int radius)
        {
            if (button.Width <= 0 || button.Height <= 0)
                return;

            int diameter = radius * 2;
            var path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, button.Width, button.Height);

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            var oldRegion = button.Region;
            button.Region = new Region(path);
            oldRegion?.Dispose();
            path.Dispose();
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                Color baseColor = (btn.Tag is Color c) ? c : btn.BackColor;
                btn.BackColor = DarkenColor(baseColor, HoverDarkenPercent);
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Color c)
            {
                btn.BackColor = c;
            }
        }

        private void Button_Resize(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                SetRoundedRegion(btn, 15);
            }
        }

        private Color DarkenColor(Color color, float percent)
        {
            percent = Clamp(percent, 0f, 1f);
            int r = (int)(color.R * (1 - percent));
            int g = (int)(color.G * (1 - percent));
            int b = (int)(color.B * (1 - percent));
            return Color.FromArgb(color.A, ClampByte(r), ClampByte(g), ClampByte(b));
        }
        private static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private int ClampByte(int v) => Math.Max(0, Math.Min(255, v));

        private void UpdateButtonState(Button startButton)
        {
            if (_organizer.IsWatching)
            {
                startButton.Text = "Остановить сортировку";
                startButton.BackColor = Color.LightCoral;

            }
            else
            {
                startButton.Text = "Запустить сортировщик";
                startButton.BackColor = Color.FromArgb(126, 201, 153);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;

                var result = MessageBox.Show(
                    "Как вы хотите закрыть приложение?\n\n" +
                    "[Да] - Закрыть приложение\n" +
                    "[Нет] - Оставить в фоне\n" +
                    "[Отмена] - Вернуться в программу",
                    "Подтверждение закрытия",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question); 

                if (result == DialogResult.Yes)
                {
                    Program._notifyIcon?.Dispose();
                    Application.Exit();
                }
                else if(result == DialogResult.No)
                {
                    this.Hide();
                }
            }
        }

        private void StandartSorterStart_button_Click(object sender, EventArgs e)
        {
            if (_organizer.IsWatching)
            {
                _organizer.StopWatching();
            }
            else
            {
                _organizer.StartWatching();

                if (SortOldFiles_checkBox.Checked)
                {
                    SortOldFiles();
                }
            }
            UpdateButtonState(StandartSorterStart_button);
        }

        private async void SortOldFiles()
        {
            StandartSorterStart_button.Enabled = false;
            SortOldFiles_checkBox.Enabled = false;

            try
            {
                var progressForm = new ProgressForm();
                progressForm.Show();

                _organizer.UpdateProgress += (current, total) =>
                {
                    progressForm.UpdateProgress(current, total);
                };

                await Task.Run(() => _organizer.SortExistingFiles());

                progressForm.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сортировке старых файлов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                StandartSorterStart_button.Enabled = true;
                SortOldFiles_checkBox.Enabled = true;
            }
        }

        private void SetupRulesPanel()
        {
            _rulesPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(159, 135, 192)
            };
            panelRules.Controls.Add(_rulesPanel);
        }

        private void LoadRules()
        {
            try
            {
                if (File.Exists("rules.json"))
                {
                    var json = File.ReadAllText("rules.json");
                    var config = JsonSerializer.Deserialize<SorterConfig>(json);
                    _rules = config.Rules;
                }
                else
                {
                    _rules = new List<SorterRule>();
                    _organizer.GenerateStandartFile();
                    LoadRules();
                }
            }
            catch
            {
                _rules = new List<SorterRule>();
            }
        }

        private void DisplayRules()
        {
            _rulesPanel.Controls.Clear();

            int yPos = 10;
            foreach (var rule in _rules)
            {
                var rulePanel = CreateRulePanel(rule, yPos);
                _rulesPanel.Controls.Add(rulePanel);
                yPos += rulePanel.Height + 5;
            }
        }

        private Panel CreateRulePanel(SorterRule rule, int yPos)
        {
            var panel = new Panel
            {
                Location = new Point(10, yPos),
                Size = new Size(panelRules.Width - 40, 40),
                BorderStyle = BorderStyle.None,
                BackColor= Color.FromArgb(159, 135, 192)
            };

            var txtExtension = new TextBox
            {
                Text = rule.Extension,
                Location = new Point(10, 10),
                Size = new Size(80, 27),
                Font = new Font("Trebuchet MS", 10, FontStyle.Bold),
                Tag = rule
            };
            txtExtension.TextChanged += (s, e) =>
            {
                rule.Extension = txtExtension.Text;
            };

            var labelArrow = new Label
            {
                Location = new Point(95, 13),
                Size = new Size(20, 13),
                BackColor = Color.Transparent
            };

            labelArrow.Paint += (s, e) =>
            {
                Image img = Properties.Resources.arrow;
                if (img == null) return;

                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImage(img, new Rectangle(0, 0, labelArrow.Width, labelArrow.Height));
            };

            var txtFolder = new TextBox
            {
                Text = rule.Folder,
                Location = new Point(120, 10),
                Size = new Size(150, 27),
                Font = new Font("Trebuchet MS", 10, FontStyle.Regular),
                Tag = rule
            };
            txtFolder.TextChanged += (s, e) =>
            {
                rule.Folder = txtFolder.Text;
            };

            var btnDelete = new Button
            {
                Text = "",
                Location = new Point(280, 9),
                Size = new Size(25, 25),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Trebuchet MS", 8, FontStyle.Regular),
                Tag = rule,
                BackColor = Color.Transparent,
                FlatAppearance =
                {
                    BorderSize = 0, 
                    MouseOverBackColor = Color.IndianRed,
                    MouseDownBackColor = Color.Firebrick
                }
            };

            // Отрисовка иконки вручную с отступами
            btnDelete.Paint += (s, e) =>
            {
                var img = Properties.Resources.delete;
                if (img == null) return;

                const int margin = 2; // отступы по 2px со всех сторон
                Rectangle area = new Rectangle(
                    margin,
                    margin,
                    btnDelete.Width - margin * 2,
                    btnDelete.Height - margin * 2
                );

                // Поддерживаем пропорции картинки
                float scale = Math.Min(
                    (float)area.Width / img.Width,
                    (float)area.Height / img.Height);

                int width = (int)(img.Width * scale);
                int height = (int)(img.Height * scale);
                int x = area.X + (area.Width - width) / 2;
                int y = area.Y + (area.Height - height) / 2;

                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImage(img, x, y, width, height);
            };

            // Добавляем обработчики для анимации при наведении
            btnDelete.MouseEnter += (s, e) =>
            {
                btnDelete.BackColor = Color.IndianRed;
                btnDelete.Cursor = Cursors.Hand;
            };

            btnDelete.MouseLeave += (s, e) =>
            {
                btnDelete.BackColor = Color.Transparent;
                btnDelete.Cursor = Cursors.Default;
            };

            btnDelete.MouseDown += (s, e) =>
            {
                btnDelete.BackColor = Color.Firebrick;
            };

            btnDelete.MouseUp += (s, e) =>
            {
                btnDelete.BackColor = Color.Transparent;
            };
            btnDelete.Click += (s, e) =>
            {
                if (MessageBox.Show($"Удалить правило {rule.Extension}?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _rules.Remove(rule);
                    SaveRules();
                    DisplayRules(); // Обновляем отображение
                }
            };

            panel.Controls.Add(txtExtension);
            panel.Controls.Add(labelArrow);
            panel.Controls.Add(txtFolder);
            panel.Controls.Add(btnDelete);

            return panel;
        }

        private void btnAddRule_Click(object sender, EventArgs e)
        {
            var extension = txtNewExtension.Text.Trim();
            var folder = txtNewFolder.Text.Trim();

            if (string.IsNullOrEmpty(extension) || string.IsNullOrEmpty(folder))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            extension = extension.ToLower();

            bool ruleExists = _rules.Any(rule =>
                rule.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase));

            if (ruleExists)
            {
                var result = MessageBox.Show(
                    $"Правило для расширения {extension} уже существует.\nЗаменить его?",
                    "Правило уже существует",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var existingRule = _rules.FirstOrDefault(rule =>
                        rule.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase));
                    if (existingRule != null)
                    {
                        _rules.Remove(existingRule);
                    }
                }
                else
                {
                    return;
                }
            }

            var newRule = new SorterRule
            {
                Extension = extension,
                Folder = folder
            };

            _rules.Add(newRule);
            SaveRules();
            DisplayRules();

            txtNewExtension.Clear();
            txtNewFolder.Clear();
            txtNewExtension.Focus();
        }

        private void btnSaveRules_Click(object sender, EventArgs e)
        {
            SaveRules();
            MessageBox.Show("Правила сохранены!", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (_organizer.IsWatching)
            {
                _organizer.StopWatching();
                _organizer.StartWatching();
                UpdateButtonState(StandartSorterStart_button);
            }
        }

        private void SaveRules()
        {
            var config = new SorterConfig { Rules = _rules };
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(config, options);
            File.WriteAllText("rules.json", json);
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/lilguch/DownloadSorter/blob/main/README.md");
        }
    }
}
