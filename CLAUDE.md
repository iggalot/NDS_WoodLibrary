# NDS Wood Library: notes for assistants

- **Read first:** `..\DesignHub\docs\SUITE.md`: all seven repos, how they fit, the branches, the build, and **the
  owner's rules** (local commits only, no installs, stay in the project folders, never click the owner's windows).
- **Branch:** `dev/nds2018`. The other repos are on `dev/ui-pass-2`.
- **What this is:** NDS wood design values and equations, used by `..\WoodFraming` and the Shear Wall Calculator.
  - `NDS2018\` (namespace `NDS_WoodLibrary.NDS2018`):
    - species and G;
    - Table 4A / 4B reference values and size factors, 2x4 to 2x12 (`Table4.Sizes`, `SizesOf`; Southern Pine's
      2x10 and 2x12 from the SFPA table kept in `docs/references`): one set for any 2x member, studs, plates, rims, floor and ceiling joists, rafters (owner 2026-11-09);
    - Table 1B sections (the same sizes);
    - adjustments: CD, Cr, CP (Eq. 3.7-1) and combined loading (Eq. 3.9-3);
    - Table 12E bolts.
    - Nails (`Nails`): the yield limit equations (Tables 12.3.1A and 12.3.1B), Fe (Table 12.3.3, as printed, to 50 psi), Fyb
      (Table 12N note 2), pmin = 6D, ℓm = p − D. Tested against Table 12N as printed. The source is AWC's NDS-2018 Ch. 12 in
      `C:\AllenEngineering\Codes\NDS 2018` (read-only; encrypted, read by rendering its pages).
      `Nails.Common` gives Table L4's common nails (L, D, H) by pennyweight.
    - The general dowel equations with a gap (`GeneralDowel`, AWC TR12 (2015) Table 1-1; with no gap they are the NDS's):
      `Nails.SingleShearZWithGap`. Tested against TR12's printed examples. The source is in `C:\AllenEngineering\Codes\AWC TR12`.
  - The 2021 files (`NDS_2015.cs`, `MaterialProperties.cs`) still build.
- **Code-table values live here once, and are never copied into an app.** Each value is tested against the table or a
  hand calculation.
- **Build:** `MSBuild NDS_WoodLibrary.sln -restore`. It is SDK-style, net472.
- **Test:** `vstest NDS_WoodLibrary.Tests\bin\Debug\net472\NDS_WoodLibrary.Tests.dll`.
- **The suite's TODO list** is `..\ShearWallCalculator\ROADMAP.md`.
