using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Empiria.Financial.Management.Billing.Adapters;
using Empiria.Financial.Management.Billing.UseCases;
using Xunit;

namespace Empiria.Tests.Billing {
  
  /// <summary></summary>
  public class BillingImporterTests {


    [Fact]
    public void Should_Import_Billing_Xml() {

      using (var usecase = BillingImporterUseCases.UseCaseInteractor()) {

        var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        string xmlPath = Path.Combine(directory.Parent.FullName, @"Resources\FACTURA DE PRUEBA XML.xml");

        BillingDto sut = usecase.ReadBillingXmlDocument(xmlPath);
        Assert.NotNull(sut);
      }
      
    }

  }
}
