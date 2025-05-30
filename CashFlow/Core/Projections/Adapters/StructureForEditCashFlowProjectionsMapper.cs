/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                          Component : Adapters Layer                        *
*  Assembly : Empiria.CashFlow.Core.dll                    Pattern   : Structurer mapper                     *
*  Type     : StructureForEditCashFlowProjectionsMapper    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Maps structured data used for cash flow projections edition.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

using Empiria.Financial;
using Empiria.Financial.Projects;

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Maps structured data used for cash flow projections edition.</summary>
  static internal class StructureForEditCashFlowProjectionsMapper {

    static private FixedList<StructureForEditCashFlowProjections> _cache;
    static private FixedList<FinancialProject> _allProjects;
    static private FixedList<FinancialAccount> _allAccounts;

    static internal FixedList<StructureForEditCashFlowProjections> Map(FixedList<OrganizationalUnit> list) {

      if (_cache != null) {
        return _cache;
      }

      _allAccounts = BaseObject.GetFullList<FinancialAccount>()
                               .ToFixedList()
                               .FindAll(x => x.FinancialAccountType.PlaysRole(CashFlowProjection.BASE_ACCOUNT_ROLE));

      _allProjects = _allAccounts.SelectDistinct(x => x.Project);

      list = _allAccounts.SelectDistinct(x => x.OrganizationalUnit);

      _cache = list.Select(x => Map(x))
                   .ToFixedList();

      return _cache;
    }


    static private StructureForEditCashFlowProjections Map(OrganizationalUnit orgUnit) {
      var structurer = new Structurer(orgUnit);

      return new StructureForEditCashFlowProjections {
        UID = orgUnit.UID,
        Name = orgUnit.Name,

        Plans = CashFlowPlan.GetList()
                            .MapToNamedEntityList(),

        ProjectionTypes = structurer.GetProjectionCategories()
      };
    }


    private class Structurer {

      private readonly OrganizationalUnit _orgUnit;
      private readonly FixedList<FinancialProject> _projects;
      private readonly FixedList<FinancialAccount> _accounts;

      public Structurer(OrganizationalUnit orgUnit) {
        _orgUnit = orgUnit;
        _accounts = _allAccounts.FindAll(x => x.OrganizationalUnit.Equals(_orgUnit));
        _projects = _allProjects.FindAll(x => _accounts.Contains(y => y.OrganizationalUnit.Equals(_orgUnit) &&
                                                                      y.Project.Equals(x)));
      }


      internal FixedList<ProjectionTypeForEditionDto> GetProjectionCategories() {
        var orgUnitCategories = CashFlowProjectionCategory.GetList();

        return orgUnitCategories.Select(x => MapCategory(x))
                                .ToFixedList();
      }


      private ProjectionTypeForEditionDto MapCategory(CashFlowProjectionCategory projectionCategory) {
        var sources = projectionCategory.OperationSources;

        return new ProjectionTypeForEditionDto {
          UID = projectionCategory.UID,
          Name = projectionCategory.Name,
          ProjectTypes = MapBaseProjectTypes(projectionCategory),
          Sources = sources.MapToNamedEntityList()
        };
      }


      private FixedList<ProjectTypeForEditionDto> MapBaseProjectTypes(CashFlowProjectionCategory projectionCategory) {

        var projectCategories = _projects.SelectDistinct(x => x.Category);

        return projectCategories.Select(x => MapProjectType(x))
                                .ToFixedList();

      }

      private ProjectTypeForEditionDto MapProjectType(FinancialProjectCategory baseProjectCategory) {
        return new ProjectTypeForEditionDto {
          UID = baseProjectCategory.UID,
          Name = baseProjectCategory.Name,
          Projects = MapProjects(baseProjectCategory)
        };
      }

      private FixedList<ProjectionProjectForEdition> MapProjects(FinancialProjectCategory projectCategory) {

        FixedList<FinancialProject> projects = _projects.FindAll(x => x.Category.Equals(projectCategory))
                                                        .ToFixedList();

        return projects.Select(x => MapFinancialProject(x))
                       .ToFixedList();
      }


      private ProjectionProjectForEdition MapFinancialProject(FinancialProject project) {
        return new ProjectionProjectForEdition {
          UID = project.UID,
          Name = project.Name,
          Accounts = _accounts.FindAll(x => x.Project.Equals(project))
                              .MapToNamedEntityList()
        };
      }

    }  // class Structurer

  }  // class StructureForEditCashFlowProjectionsMapper

}  // namespace Empiria.CashFlow.Projections.Adapters
