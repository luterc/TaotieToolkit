using System;
using System.Threading;
using System.Windows.Forms;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather.system_info
{
    class ClipboardGet : ICommand, ICommandMarker
    {
        public string Name => "ClipboardGet";
        public string Description => "获取剪贴板数据";
        public void Execute(string[] args)
       {
            Console.WriteLine("\n[+] Get clipboard data");
            try
            {
                object ret = null;
                ThreadStart method = delegate ()
                {
                    System.Windows.Forms.IDataObject dataObject = Clipboard.GetDataObject();
                    if (dataObject != null && dataObject.GetDataPresent(DataFormats.Text))
                    {
                        ret = dataObject.GetData(DataFormats.Text);
                    }
                };
                if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
                {
                    Thread thread = new Thread(method);
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    thread.Join();
                }
                else
                {
                    method();
                }
                Console.WriteLine(ret);
            }
            catch (Exception e) {
                Console.WriteLine("\t[-] Can not get clipboard data,exception:" + e.Message);
            }
        }
    }
}
