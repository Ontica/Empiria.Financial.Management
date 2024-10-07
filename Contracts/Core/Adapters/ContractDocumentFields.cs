/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Fields DTO                              *
*  Type     : ContractDocumentFields                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO fields structure used for update contracts information.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Contracts.Adapters {

  /// <summary>DTO fields structure used for update contracts document fields information.</summary>
  public class ContractDocumentFields {

    public string ID {
      get; set;
    }


    public string UID {
      get; set;
    }


    public string Name {
      get; set;
    }


    public string SourceTypeUID {
      get; set;
    }


    public string Path {
      get; set;
    }


    public string Directory {
      get; set;
    }


    public string SourceObjectUID {
      get; set;
    }


    public string EmmitedByUID {
      get; set;
    }

    public string ExtensionData {
      get; set;
    }


    public string KeyWords {
      get; set;
    }


    public string PostedByUID {
      get; set;
    }


    public string PostingTime {
      get; set;
    }


    public string Status {
      get; set;
    }


    internal void EnsureValid() {
      // nop
    }

  }  // class ContractSupplierFields

}  // namespace Empiria.Contracts.Adapters
