using Novell.Directory.Ldap;
using System;

namespace ActiveDirectoryNetCore
{
    class Program
    {
		// Source: https://github.com/dsbenghe/Novell.Directory.Ldap.NETStandard/blob/master/original_samples/Samples/VerifyPassword.cs
        static void Main(string[] args)
        {
    		if (args.Length != 3)
	    	{
		    	Console.Out.WriteLine("Usage: <host name> <login dn> <password>");
    			Console.Out.WriteLine("Examples:");
				Console.Out.WriteLine("\tyourADserver.com \"cn=Admin,o=Acme\" secret");
				Console.Out.WriteLine("\tyourADserver.com DOMAIN\\SAMUserName secret");
	    		return;
		    }
		
		    string ldapHost = args[0],
    			   loginDN = args[1],
	    	       password = args[2];

            using (var conn = new LdapConnection())
            {
    		    try
	    	    {
    	    		// connect to the server
	    	    	conn.Connect(ldapHost, LdapConnection.DEFAULT_PORT);
			
	    	    	// authenticate to the server
        			conn.Bind(LdapConnection.Ldap_V3, loginDN, password);
    
	    		    Console.WriteLine("The password is correct.");
    		    }
	    	    catch (LdapException e)
		        {
					if (e.ResultCode == LdapException.INVALID_CREDENTIALS)
						Console.Error.WriteLine("Invalid credentials.");
    		    	else if (e.ResultCode == LdapException.NO_SUCH_OBJECT)
	    		    	Console.Error.WriteLine("Error: No such entry");
    		    	else if (e.ResultCode == LdapException.NO_SUCH_ATTRIBUTE)
	    		    	Console.Error.WriteLine("Error: No such attribute");
					else
	    	    		Console.Error.WriteLine($"Error: {e}");
		        }   
		        catch (System.IO.IOException e)
    		    {
    	    		Console.Out.WriteLine($"Error: {e}");
	    	    }
				finally
				{
		    	    // disconnect with the server
			        conn.Disconnect();
				}
            }
        }
    }
}
