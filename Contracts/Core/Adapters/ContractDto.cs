/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Data Transfer Object                    *
*  Type     : ContractDto                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return contracts information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Contracts.Adapters {

  /// <summary>Data transfer object used to return contracts information.</summary>
  public class ContractDto {


    public int ID {
      get; internal set;
    }


    public string UID {
      get; internal set;
    }


    public string ContractNo {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public string Description {
      get; internal set;
    }


    public NamedEntityDto Currency {
      get; internal set;
    }


    public DateTime FromDate {
      get; internal set;
    }


    public DateTime ToDate {
      get; internal set;
    }


    public DateTime SignDate {
      get; internal set;
    }


    public NamedEntityDto ManagedByOrgUnit {
      get; internal set;
    }


    public NamedEntityDto Supplier {
      get; internal set;
    }


    public NamedEntityDto BudgetType {
      get; internal set;
    }


    public string ParentContract {
      get; internal set;
    }


    public string ExtData {
      get; internal set;
    }


    public string KeyWords {
      get; internal set;
    }


    public NamedEntityDto PostedBy {
      get; internal set;
    }


    public string PostingTime {
      get; internal set;
    }


    public string Status {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }

  }  // class ContractDto


  /// Output Dto used to return minimal contract data
  public class ContractDescriptor {

    public string UID {
      get; internal set;
    }


    public string ContractNo {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public string Description {
      get; internal set;
    }


    public DateTime SignDate {
      get; internal set;
    }


    public string Supplier {
      get; internal set;
    }


    public string BudgetType {
      get; internal set;
    }


    public string ManagedByOrgUnit {
      get; internal set;
    }


    public string ContractType {
      get; internal set;
    }


    public string Currency {
      get; internal set;
    }


    public DateTime FromDate {
      get; internal set;
    }


    public DateTime ToDate {
      get; internal set;
    }


    public string statusName {
      get; internal set;
    }

  } // class ContractDescriptor

}  // namespace Empiria.Contracts.Adapters
