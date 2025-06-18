/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Domain Layer                          *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input fields                          *
*  Type     : FinancialProjectFields                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields with financial projects data.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Newtonsoft.Json;

using Empiria.Parties;

namespace Empiria.Financial.Projects {

  /// <summary>Input fields with financial projects data.</summary>
  public class FinancialProjectFields {

    public FinancialProjectFields() {
      JsonConvert.DefaultSettings = () => Json.JsonConverter.JsonSerializerDefaultSettings();
    }

    #region Properties

    [Newtonsoft.Json.JsonProperty("ProjectTypeUID")]
    public string ProjectTypeCategoryUID {
      get; set;
    } = string.Empty;


    public string SubprogramUID {
      get; set;
    } = string.Empty;


    public string BaseOrgUnitUID {
      get; set;
    } = string.Empty;


    public string ProjectNo {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    public string AssigneeUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string Justification {
      get; set;
    } = string.Empty;


    public object ProjectGoals {
      get; set;
    } = new object();

    #endregion Properties

    #region Methods

    internal void EnsureValid() {
      ProjectNo = EmpiriaString.Clean(ProjectNo);
      Name = EmpiriaString.Clean(Name);

      ProjectTypeCategoryUID = Patcher.CleanUID(ProjectTypeCategoryUID);
      SubprogramUID = Patcher.CleanUID(SubprogramUID);
      BaseOrgUnitUID = Patcher.CleanUID(BaseOrgUnitUID);
      AssigneeUID = Patcher.CleanUID(AssigneeUID);

      if (ProjectTypeCategoryUID.Length != 0) {
        _ = FinancialProjectCategory.Parse(ProjectTypeCategoryUID);
      }

      if (SubprogramUID.Length != 0) {
        _ = StandardAccount.Parse(SubprogramUID);
      }

      if (BaseOrgUnitUID.Length != 0) {
        _ = OrganizationalUnit.Parse(BaseOrgUnitUID);
      }

      if (AssigneeUID.Length != 0) {
        _ = Person.Parse(AssigneeUID);
      }

    }

    #endregion Methods

  }  // class FinancialProjectFields

}  // namespace Empiria.Financial.Projects
