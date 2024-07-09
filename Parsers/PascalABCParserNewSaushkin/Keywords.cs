// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;

namespace Languages.Pascal.Frontend.Core
{

    public class PascalABCKeywords : PascalABCCompiler.Parsers.BaseKeywords
    {

        protected override string FileName => "keywordsmap.pabc";

        protected override Dictionary<string, int> KeywordsToTokens { get; set; } 
            
        public PascalABCKeywords() : base()
        {
            KeywordsToTokens = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase)
            {
                ["or"] = (int)Tokens.tkOr,
                ["xor"] = (int)Tokens.tkXor,
                ["and"] = (int)Tokens.tkAnd,
                ["div"] = (int)Tokens.tkDiv,
                ["mod"] = (int)Tokens.tkMod,
                ["shl"] = (int)Tokens.tkShl,
                ["shr"] = (int)Tokens.tkShr,
                ["not"] = (int)Tokens.tkNot,
                ["as"] = (int)Tokens.tkAs,
                ["in"] = (int)Tokens.tkIn,
                ["is"] = (int)Tokens.tkIs,
                ["implicit"] = (int)Tokens.tkImplicit,
                ["explicit"] = (int)Tokens.tkExplicit,
                ["sizeof"] = (int)Tokens.tkSizeOf,
                ["typeof"] = (int)Tokens.tkTypeOf,
                ["where"] = (int)Tokens.tkWhere,
                ["array"] = (int)Tokens.tkArray,
                ["begin"] = (int)Tokens.tkBegin,
                ["case"] = (int)Tokens.tkCase,
                ["class"] = (int)Tokens.tkClass,
                ["const"] = (int)Tokens.tkConst,
                ["constructor"] = (int)Tokens.tkConstructor,
                ["default"] = (int)Tokens.tkDefault,
                ["destructor"] = (int)Tokens.tkDestructor,
                ["downto"] = (int)Tokens.tkDownto,
                ["do"] = (int)Tokens.tkDo,
                ["else"] = (int)Tokens.tkElse,
                ["end"] = (int)Tokens.tkEnd,
                ["event"] = (int)Tokens.tkEvent,
                ["except"] = (int)Tokens.tkExcept,
                ["exports"] = (int)Tokens.tkExports,
                ["file"] = (int)Tokens.tkFile,
                ["finalization"] = (int)Tokens.tkFinalization,
                ["finally"] = (int)Tokens.tkFinally,
                ["for"] = (int)Tokens.tkFor,
                ["foreach"] = (int)Tokens.tkForeach,
                ["function"] = (int)Tokens.tkFunction,
                ["goto"] = (int)Tokens.tkGoto,
                ["if"] = (int)Tokens.tkIf,
                ["implementation"] = (int)Tokens.tkImplementation,
                ["inherited"] = (int)Tokens.tkInherited,
                ["initialization"] = (int)Tokens.tkInitialization,
                ["interface"] = (int)Tokens.tkInterface,
                ["label"] = (int)Tokens.tkLabel,
                ["lock"] = (int)Tokens.tkLock,
                ["loop"] = (int)Tokens.tkLoop,
                ["nil"] = (int)Tokens.tkNil,
                ["procedure"] = (int)Tokens.tkProcedure,
                ["of"] = (int)Tokens.tkOf,
                ["operator"] = (int)Tokens.tkOperator,
                ["property"] = (int)Tokens.tkProperty,
                ["raise"] = (int)Tokens.tkRaise,
                ["record"] = (int)Tokens.tkRecord,
                ["repeat"] = (int)Tokens.tkRepeat,
                ["set"] = (int)Tokens.tkSet,
                ["try"] = (int)Tokens.tkTry,
                ["type"] = (int)Tokens.tkType,
                ["then"] = (int)Tokens.tkThen,
                ["to"] = (int)Tokens.tkTo,
                ["until"] = (int)Tokens.tkUntil,
                ["uses"] = (int)Tokens.tkUses,
                ["var"] = (int)Tokens.tkVar,
                ["while"] = (int)Tokens.tkWhile,
                ["with"] = (int)Tokens.tkWith,
                ["program"] = (int)Tokens.tkProgram,
                ["template"] = (int)Tokens.tkTemplate,
                ["resourcestring"] = (int)Tokens.tkResourceString,
                ["threadvar"] = (int)Tokens.tkThreadvar,
                ["sealed"] = (int)Tokens.tkSealed,
                ["partial"] = (int)Tokens.tkPartial,
                ["params"] = (int)Tokens.tkParams,
                ["unit"] = (int)Tokens.tkUnit,
                ["library"] = (int)Tokens.tkLibrary,
                ["external"] = (int)Tokens.tkExternal,
                ["name"] = (int)Tokens.tkName,
                ["private"] = (int)Tokens.tkPrivate,
                ["protected"] = (int)Tokens.tkProtected,
                ["public"] = (int)Tokens.tkPublic,
                ["internal"] = (int)Tokens.tkInternal,
                ["read"] = (int)Tokens.tkRead,
                ["write"] = (int)Tokens.tkWrite,
                ["on"] = (int)Tokens.tkOn,
                ["forward"] = (int)Tokens.tkForward,
                ["abstract"] = (int)Tokens.tkAbstract,
                ["overload"] = (int)Tokens.tkOverload,
                ["reintroduce"] = (int)Tokens.tkReintroduce,
                ["override"] = (int)Tokens.tkOverride,
                ["virtual"] = (int)Tokens.tkVirtual,
                ["extensionmethod"] = (int)Tokens.tkExtensionMethod,
                ["new"] = (int)Tokens.tkNew,
                ["auto"] = (int)Tokens.tkAuto,
                ["sequence"] = (int)Tokens.tkSequence,
                ["yield"] = (int)Tokens.tkYield,
                ["match"] = (int)Tokens.tkMatch,
                ["when"] = (int)Tokens.tkWhen,
                ["namespace"] = (int)Tokens.tkNamespace,
                ["static"] = (int)Tokens.tkStatic,
                ["step"] = (int)Tokens.tkStep,
                ["index"] = (int)Tokens.tkIndex,
                ["async"] = (int)Tokens.tkAsync,
                ["await"] = (int)Tokens.tkAwait
            }
            .ToDictionary(kv => ConvertKeyword(kv.Key), kv => kv.Value);
        }

        protected override int GetIdToken() => (int)Tokens.tkIdentifier;
    }

}
