﻿
module AssemblyLevelDeclarations

open System.Reflection
open System.Security
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

[<assembly: AssemblyVersion("0.0.1.0")>]
[<assembly: AssemblyFileVersion("0.0.1.0")>]
//[<assembly: InternalsVisibleTo("Castle.MonoRail.Tests, PublicKey=002400000480000094000000060200000024000052534131000400000100010077f5e87030dadccce6902c6adab7a987bd69cb5819991531f560785eacfc89b6fcddf6bb2a00743a7194e454c0273447fc6eec36474ba8e5a3823147d214298e4f9a631b1afee1a51ffeae4672d498f14b000e3d321453cdd8ac064de7e1cf4d222b7e81f54d4fd46725370d702a05b48738cc29d09228f1aa722ae1a9ca02fb") >]

// [<assembly: AllowPartiallyTrustedCallers()>]
// [<assembly: SecurityRules(SecurityRuleSet.Level2)>]
// http://msdn.microsoft.com/en-us/library/dd233102.aspx
//[<assembly: SecurityCritical()>] // means we're transparent but may expose critical api

do()