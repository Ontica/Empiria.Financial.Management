/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Adapters Layer                          *
*  Assembly : Empiria.ContractMilestone.Core.dll         Pattern   : Query DTO                               *
*  Type     : ContractMilestoneQuery                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query data transfer object used to search contracts milestone.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Contracts.Adapters {

  /// <summary>Query data transfer object used to search contracts milestone.</summary>
  public class ContractMilestoneQuery {

    public string ContractNo {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public string SupplierUID {
      get; set;
    } = string.Empty;


    public string BudgetTypeUID {
      get; set;
    } = string.Empty;

    
    public string ManagedByOrgUnitUID {
      get; set;
    } = string.Empty;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;


    public string OrderBy {
      get; set; 
    } = string.Empty;

  }  // class ContractMilestoneQuery

} // namespace Empiria.Contracts.Adapters