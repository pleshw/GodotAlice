
using System.Net;
using System.Net.Sockets;

namespace Multiplayer;

public static class NetworkController
{
  public readonly static int DefaultPort = 8910;
  public static string Address
  {
    get
    {
      return "localhost";
      // Get all local IP addresses of the host PC
      IPAddress[]
      localIPs = Dns.GetHostAddresses(Dns.GetHostName());

      // Filter and return the first IPv4 address (you may adjust this logic based on your requirements)
      foreach (IPAddress localIP in localIPs)
      {
        if (localIP.AddressFamily == AddressFamily.InterNetwork)
        {
          return localIP.ToString();
        }
      }

      // If no IPv4 address is found, return an empty string or handle the situation accordingly
      return "";
    }
  }
}