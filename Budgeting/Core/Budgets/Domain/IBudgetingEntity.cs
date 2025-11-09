/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Interface                               *
*  Type     : IBudgetingEntity                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Marker interface for budgeting entities.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;

namespace Empiria.Budgeting {

  /// <summary>Marker interface for budgeting entities.</summary>
  public interface IBudgetingEntity : IIdentifiable {

    ObjectTypeInfo GetEmpiriaType();

  } // interface IBudgetingEntity

}  // namespace Empiria.Budgeting
