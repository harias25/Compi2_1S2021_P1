using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Gramaticas;
using Irony;
using Irony.Parsing;
using ScintillaNET;

namespace Compiladores2_LabProyecto1
{
    public partial class Form1 : Form
    {
        public static RichTextBox Consola;
        ScintillaNET.Scintilla TextArea;
		private const int BACK_COLOR = 0x2A211C;
		private const int FORE_COLOR = 0xB7B7B7;
		private const int NUMBER_MARGIN = 1;
		private const int BOOKMARK_MARGIN = 2;
		private const int BOOKMARK_MARKER = 2;
		private const int FOLDING_MARGIN = 3;
		private const bool CODEFOLDING_CIRCULAR = true;

		public Form1()
        {
            InitializeComponent();
            Consola = richTextBox1;

            TextArea = new ScintillaNET.Scintilla();
            TextPanel.Controls.Add(TextArea);

            TextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            TextArea.WrapMode = WrapMode.None;
            TextArea.IndentationGuides = IndentView.LookBoth;
            TextArea.SetSelectionBackColor(true, IntToColor(0x114D9C));
			InitSyntaxColoring();
			InitNumberMargin();
			InitBookmarkMargin();
			InitCodeFolding();
		}


		private void InitCodeFolding()
		{

			TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
			TextArea.SetFoldMarginHighlightColor(true, IntToColor(BACK_COLOR));

			// Enable code folding
			TextArea.SetProperty("fold", "1");
			TextArea.SetProperty("fold.compact", "1");

			// Configure a margin to display folding symbols
			TextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
			TextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
			TextArea.Margins[FOLDING_MARGIN].Sensitive = true;
			TextArea.Margins[FOLDING_MARGIN].Width = 20;

			// Set colors for all folding markers
			for (int i = 25; i <= 31; i++)
			{
				TextArea.Markers[i].SetForeColor(IntToColor(BACK_COLOR)); // styles for [+] and [-]
				TextArea.Markers[i].SetBackColor(IntToColor(FORE_COLOR)); // styles for [+] and [-]
			}

			// Configure folding markers with respective symbols
			TextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
			TextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
			TextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
			TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
			TextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
			TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
			TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

			// Enable automatic folding
			TextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

		}

		private void InitBookmarkMargin()
		{

			//TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));

			var margin = TextArea.Margins[BOOKMARK_MARGIN];
			margin.Width = 20;
			margin.Sensitive = true;
			margin.Type = MarginType.Symbol;
			margin.Mask = (1 << BOOKMARK_MARKER);
			//margin.Cursor = MarginCursor.Arrow;

			var marker = TextArea.Markers[BOOKMARK_MARKER];
			marker.Symbol = MarkerSymbol.Circle;
			marker.SetBackColor(IntToColor(0xFF003B));
			marker.SetForeColor(IntToColor(0x000000));
			marker.SetAlpha(100);

		}

		private Color IntToColor(int rgb)
		{
			return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
		}

		private void InitNumberMargin()
		{

			TextArea.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
			TextArea.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
			TextArea.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
			TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

			var nums = TextArea.Margins[NUMBER_MARGIN];
			nums.Width = 30;
			nums.Type = MarginType.Number;
			nums.Sensitive = true;
			nums.Mask = 0;

			TextArea.MarginClick += TextArea_MarginClick;
		}

		private void InitSyntaxColoring()
		{

			// Configure the default style
			TextArea.StyleResetDefault();
			TextArea.Styles[Style.Default].Font = "Consolas";
			TextArea.Styles[Style.Default].Size = 10;
			TextArea.Styles[Style.Default].BackColor = IntToColor(0x212121);
			TextArea.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
			TextArea.StyleClearAll();

			// Configure the CPP (C#) lexer styles
			TextArea.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0xD0DAE2);
			TextArea.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
			TextArea.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
			TextArea.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
			TextArea.Styles[Style.Cpp.Number].ForeColor = IntToColor(0xFFFF00);
			TextArea.Styles[Style.Cpp.String].ForeColor = IntToColor(0xFFFF00);
			TextArea.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
			TextArea.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
			TextArea.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0xE0E0E0);
			TextArea.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
			TextArea.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
			TextArea.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
			TextArea.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
			TextArea.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
			TextArea.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
			TextArea.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);

			TextArea.Lexer = Lexer.Cpp;

			TextArea.SetKeywords(0, "print println if else for while switch case default int string double boolean");
			TextArea.SetKeywords(1, "True False");

		}

		private void TextArea_MarginClick(object sender, MarginClickEventArgs e)
		{
			if (e.Margin == BOOKMARK_MARGIN)
			{
				// Do we have a marker for this line?
				const uint mask = (1 << BOOKMARK_MARKER);
				var line = TextArea.Lines[TextArea.LineFromPosition(e.Position)];
				if ((line.MarkerGet() & mask) > 0)
				{
					// Remove existing bookmark
					line.MarkerDelete(BOOKMARK_MARKER);
				}
				else
				{
					// Add bookmark
					line.MarkerAdd(BOOKMARK_MARKER);
				}
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (!TextArea.Text.Equals(string.Empty))
			{
				Consola.Text = "";
				Gramatica grammar = new Gramatica();
				LanguageData lenguaje = new LanguageData(grammar);
				Parser parser = new Parser(lenguaje);
				ParseTree arbol = parser.Parse(TextArea.Text);

				if (arbol.ParserMessages.Count != 0)
				{
					MessageBox.Show("Se han encontrado errores", "Errores",
									 MessageBoxButtons.OK,
									 MessageBoxIcon.Error);

					List<LogMessage> errores = arbol.ParserMessages;
					foreach (LogMessage error in errores)
					{
						if (error.Message.Contains("Sintax"))
						{
							Consola.AppendText("Error Sintactico, " + error.Message + " Linea: " + error.Location.Line + ", Columna: " + error.Location.Column);
						}
						else
						{
							Consola.AppendText("Error Lexico, " + error.Message + " Linea: " + error.Location.Line + ", Columna: " + error.Location.Column);
						}
					}
                }
                else
                {
					
					GeneradorAST generadorAST = new GeneradorAST(arbol);
					AST ast = generadorAST.arbol;
					Entorno ent = new Entorno(null);

					if (ast != null)
					{
						foreach (Instruccion ins in ast.Instrucciones)
						{
							ins.ejecutar(ent, ast);
						}

						Graficador j = new Graficador();
						j.graficar(arbol.Root);

					}
					else
					{
						MessageBox.Show("Error generando el AST", "Errores",
										 MessageBoxButtons.OK,
										 MessageBoxIcon.Error);
					}
				}
			}
		}

        private void button2_Click(object sender, System.EventArgs e)
        {
			Consola.Text = "";
        }
    }
}
