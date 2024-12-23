﻿/* Empiria Financial *****************************************************************************************
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
      get; set;
    }


    public string BillNo {
      get; set;
    }


    public string BillTypeName {
      get; set;
    }


    public string IssuedByName {
      get; set;
    }


    public string IssuedToName {
      get; set;
    }


    public string CategoryName {
      get; set;
    }


    public decimal Total {
      get; set;
    }


    public DateTime IssueDate {
      get; set;
    }


    public string StatusName {
      get; set;
    }
    
  } // class BillDescriptorDto

} // namespace Empiria.Billing.Adapters
