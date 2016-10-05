using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace XmlLongLinesSplitter
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1
	{
		public Window1()
		{
			InitializeComponent();
		}





        //
        // Hi my beloved interviewers. Do not judge my XAML and C# strictly here. It's a sketch app of 2010 year.
        //




















        private void Button1Click(object sender, RoutedEventArgs e)
		{
			var openDialog = new OpenFileDialog();
			if (openDialog.ShowDialog() == false)
			{
				return;
			}
			if(1 < openDialog.FileNames.Length)
			{
                // Checking for curious cases.
                return;
			}

			var saveDialog = new SaveFileDialog();
			saveDialog.ShowDialog();
			if (saveDialog.FileName=="")
			{
				return;
			}
			if (1 < saveDialog.FileNames.Length)
			{
				// Checking for curious cases.
				return;
			}

			Stream sourcestream;
			string tempFileName = null;
			if(openDialog.FileName != saveDialog.FileName)
			{
				sourcestream = openDialog.OpenFile();
			}
			else
			{
				// Requested saving to the source file.
				tempFileName = Path.ChangeExtension(openDialog.FileName, ".tmp");
				
				File.Move(openDialog.FileName, tempFileName);
				sourcestream = File.OpenRead(tempFileName);
			}



            // TODO.it3xl.com: Recheck Win-1251 & UTF-8 processing.






			//new FileInfo(openDialog.FileName);
			using (sourcestream )
			{
				using (var targetStream = saveDialog.OpenFile())
				{
					var source = sourcestream;
					var target = targetStream;

                    //while (reader.Peek() != -1))
                    while (true)
					{
                        var currentByte = source.ReadByte();
                        if (currentByte == -1)
                        {
                            break;
                        }
                        target.WriteByte((byte)currentByte);
                        var currentChar = (char)currentByte;
						if (currentChar != '>')
						{
							continue;
						}

                        var nextByte = source.ReadByte();
                        if (nextByte == -1)
                        {
                            break;
                        }
                        var nextChar = (char)nextByte;
						if (nextChar == '<')
						{
							target.WriteByte((byte)'\n');
						}
						target.WriteByte((byte)nextChar);
					}

					// Flushes out buffer of the stream.
					target.Flush();
				}
			}

			if (tempFileName!=null)
			{
				try
				{
					File.Delete(tempFileName);
				}
				catch (Exception)
				{
				    MessageBox.Show($"Attention!\n\nCan't delete temp file\n\n{tempFileName}");
				}
			}

			MessageBox.Show(this, "Adding line breaks was successful.");
		}
	}
}
