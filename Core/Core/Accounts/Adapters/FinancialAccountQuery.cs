/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Query Data Transfer Object              *
*  Type     : FinancialAccountQuery                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to search financial accounts.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Financial.Adapters {

  /// <summary>Query DTO used to search financial accounts.</summary>
  public class FinancialAccountQuery {

    public string OrganizationUnitUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;


    public string OrderBy {
      get; set;
    } = string.Empty;


  }  // class FinancialAccountQuery

}  // namespace Empiria.Financial.Adapters
