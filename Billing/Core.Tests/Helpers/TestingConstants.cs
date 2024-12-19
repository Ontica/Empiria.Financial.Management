/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Test Helpers                            *
*  Assembly : Empiria.Billing.Core.Tests.dll             Pattern   : Testing constants                       *
*  Type     : TestingConstants                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides testing constants for Empiria Billing Management core module.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.IO;

namespace Empiria.Tests.Billing {

  /// <summary>Provides testing constants for Empiria Billing Management core module.</summary>
  static public class TestingConstants {

    static public string XML_BILL_FILE_PATH {
      get {
        var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        return Path.Combine(directory.Parent.FullName, @"Resources\factura.xml");
      }
    }


    static public string XML_CREDIT_NOTE_FILE_PATH {
      get {
        var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        return Path.Combine(directory.Parent.FullName, @"Resources\nota_credito2.xml");
      }
    }

  }  // class TestingConstants

}  // namespace Empiria.Tests.Billing
