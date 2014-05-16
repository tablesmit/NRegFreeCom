
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using NUnit.Framework;
using NRegFreeCom;
namespace NRegFreeCom.IntegrationTests
{
    [TestFixture]
    public class RegAsmTests
    {
  
    	[SetUp] 
    	public void Init()
        {
    		RegAsm.User.RegisterInterfaces(ComTypes.ComInterfaces.Take((int)20), RegistryView.Default);
    	}
    	
    	[TearDown]
    	public void Cleanup(){
    		per_item_unregistration(20);
    	}
    
        [Test]
        public void User_RegisterInterface_batchIsUsefull()
        {        	
        	var results = NStopwatch.SpeedTesting.Do(Console.Out,10,20,         	                           
        	                           (r)=> batch_registration(r),
        	                           (r)=> per_item_registration(r)
        	                          );
        	Assert.IsTrue(results[0].TotalRunningTime < results[1].TotalRunningTime, "Batch registration expected to be faster then per item");
            
        }
        
        private void batch_registration(long count){
        	using (new NStopwatch.ExcludeTime()){
        		per_item_unregistration(count);
        	}
        	
        	RegAsm.User.RegisterInterfaces(ComTypes.ComInterfaces.Take((int)count), RegistryView.Default);
        	
        }
        
        
        private void per_item_registration(long count){
        	using (new NStopwatch.ExcludeTime()){
        		per_item_unregistration(count);
        	}
        	for (int i = 0; i < count; i++) {        	
        		RegAsm.User.RegisterInterface(ComTypes.ComInterfaces[i], RegistryView.Default);
        	}
        }
        
        private void per_item_unregistration(long count){
        	for (int i = 0; i < count; i++) {        	
        		RegAsm.User.UnregisterInterface(ComTypes.ComInterfaces[i], RegistryView.Default);
        	}
        }
        
        [Test]
        public void User_UnregisterTypeLib_Ok()
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            RegAsm.User.UnregisterTypeLib(asm, RegistryView.Default);
        }
    }
}
