/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Query Data Transfer Object              *
*  Type     : FinancialProjectQuery                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to search financial projects.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Query DTO used to search financial projects.</summary>
  public class FinancialProjectQuery {

    public string BaseOrgUnitUID {
      get; set;
    } = string.Empty;


    [Newtonsoft.Json.JsonProperty(PropertyName = "ProjectionTypeUID")]
    public string ProjectTypeCategoryUID {
      get; set;
    } = string.Empty;


    public string ProgramUID {
      get; set;
    } = string.Empty;


    public string SubprogramUID {
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

  }  // class FinancialProjectQuery

}  // namespace Empiria.Financial.Projects.Adapters
