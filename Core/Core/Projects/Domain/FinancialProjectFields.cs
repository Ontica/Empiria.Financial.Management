/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Domain Layer                          *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input fields                          *
*  Type     : FinancialProjectFields                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields with financial projects data.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Financial.Projects {

  /// <summary>Input fields with financial projects data.</summary>
  public class FinancialProjectFields {

    #region Properties

    public string StandardAccountUID {
      get; set;
    } = string.Empty;


    public string CategoryUID {
      get; set;
    } = string.Empty;


    public string ProjectNo {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    public string OrganizationUnitUID {
      get; set;
    } = string.Empty;


    #endregion Properties

    #region Methods

    internal void EnsureValid() {
      ProjectNo = FieldPatcher.Clean(ProjectNo);
      Name = FieldPatcher.Clean(Name);

      CategoryUID = FieldPatcher.Clean(CategoryUID);
      StandardAccountUID = FieldPatcher.Clean(StandardAccountUID);
      OrganizationUnitUID = FieldPatcher.Clean(OrganizationUnitUID);

      if (CategoryUID.Length != 0) {
        _ = FinancialProjectCategory.Parse(CategoryUID);
      }

      if (StandardAccountUID.Length != 0) {
        _ = StandardAccount.Parse(StandardAccountUID);
      }

      if (OrganizationUnitUID.Length != 0) {
        _ = StandardAccount.Parse(OrganizationUnitUID);
      }
    }


    internal OrganizationalUnit GetOrganizationalUnit() {
      return OrganizationalUnit.Parse(OrganizationUnitUID);
    }

    #endregion Methods

  }  // class FinancialProjectFields

}  // namespace Empiria.Financial.Projects
