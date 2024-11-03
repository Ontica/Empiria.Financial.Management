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

using Empiria.Documents.Services.Adapters;
using Empiria.History.Services.Adapters;

namespace Empiria.Contracts.Adapters {

  public class ContractHolderDto {

    public ContractDto Contract {
      get; internal set;
    }

    public FixedList<ContractItemDto> Items {
      get; internal set;
    }

    public FixedList<ContractMilestoneDto> Milestones {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryDto> History {
      get; internal set;
    }

  }  // class ContractHolderDto


  /// <summary>Data transfer object used to return contracts information.</summary>
  public class ContractDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto ContractType {
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

    public NamedEntityDto Supplier {
      get; internal set;
    }

    public NamedEntityDto ManagedByOrgUnit {
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

    public NamedEntityDto BudgetType {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }


  }  // class ContractDto


  /// Output Dto used to return minimal contract data
  public class ContractDescriptor {

    public string UID {
      get; internal set;
    }

    public string ContractType {
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

    public string Supplier {
      get; internal set;
    }

    public string ManagedByOrgUnit {
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

    public string BudgetType {
      get; internal set;
    }

    public string Currency {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }


  } // class ContractDescriptor

}  // namespace Empiria.Contracts.Adapters
