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

    static internal FixedList<StructureForEditCashFlowProjections> Map(FixedList<OrganizationalUnit> list) {
      return list.Select(x => Map(x))
                 .ToFixedList();
    }

    static private StructureForEditCashFlowProjections Map(OrganizationalUnit orgUnit) {
      var helper = new Structurer(orgUnit);
      return new StructureForEditCashFlowProjections {
        UID = orgUnit.UID,
        Name = orgUnit.Name,

        Plans = CashFlowPlan.GetList()
                            .MapToNamedEntityList(),

        ProjectionTypes = helper.GetProjectionCategories()
      };
    }


    private class Structurer {

      private readonly OrganizationalUnit _orgUnit;
      private readonly FixedList<FinancialProject> _projects;
      private readonly FixedList<FinancialAccount> _accounts;

      public Structurer(OrganizationalUnit orgUnit) {
        _orgUnit = orgUnit;
        _accounts = BaseObject.GetFullList<FinancialAccount>()
                              .ToFixedList()
                              .FindAll(x => x.OrganizationalUnit.Equals(_orgUnit));
        _projects = BaseObject.GetFullList<FinancialProject>()
                              .ToFixedList()
                              .FindAll(x => _accounts.Contains(y => y.OrganizationalUnit.Equals(_orgUnit)));

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
        var projects = _projects.FindAll(x => x.Accounts.Contains(y => y.Project.Equals(x) &&
                                                                       y.OrganizationalUnit.Equals(_orgUnit)));


        var projectCategories = projects.SelectDistinct(x => x.Category);

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
          Accounts = project.Accounts.MapToNamedEntityList()
        };
      }

    }  // class Structurer

  }  // class StructureForEditCashFlowProjectionsMapper

}  // namespace Empiria.CashFlow.Projections.Adapters
