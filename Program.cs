using Microsoft.Win32;
using System;

namespace UAC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string regRuta = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
            string regNombre = "EnableLUA";

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regRuta, writable: true))
                {
                    if (key == null)
                    {
                        Console.WriteLine("No se pudo acceder al registro. Ejecuta como administrador !!!");
                        return;
                    }

                    // Si no hay parámetros, mostrar estado actual
                    if (args.Length == 0)
                    {
                        object value = key.GetValue(regNombre);
                        if (value != null && (int)value == 1)
                        {
                            Console.WriteLine("UAC está actualmente ACTIVADO.");
                        }
                        else
                        {
                            Console.WriteLine("UAC está actualmente DESACTIVADO.");
                        }
                        return;
                    }

                    // Procesar parámetro
                    string action = args[0].ToUpper();

                    switch (action)
                    {
                        case "/ON":
                            key.SetValue(regNombre, 1, RegistryValueKind.DWord);
                            Console.WriteLine("UAC ha sido ACTIVADO. Reinicia el sistema para aplicar cambios.");
                            break;

                        case "/OFF":
                            key.SetValue(regNombre, 0, RegistryValueKind.DWord);
                            Console.WriteLine("UAC ha sido DESACTIVADO. Reinicia el sistema para aplicar cambios.");
                            break;

                        default:
                            Console.WriteLine("Parámetro inválido. Usa /ON o /OFF.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
