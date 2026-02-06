/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Mapping class                           *
*  Type     : BillDescriptorDto                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return bill descriptor data.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Billing.Adapters {

  /// <summary>Output DTO used to return bill descriptor data.</summary>
  public class BillDescriptorDto {

    public string UID {
      get; internal set;
    }


    public string BillNo {
      get; internal set;
    }


    public string BillTypeName {
      get; internal set;
    }


    public string IssuedByName {
      get; internal set;
    }


    public string IssuedToName {
      get; internal set;
    }


    public string CategoryName {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }


    public DateTime IssueDate {
      get; internal set;
    }


    public string StatusName {
      get; internal set;
    }
    
  } // class BillDescriptorDto

} // namespace Empiria.Billing.Adapters
