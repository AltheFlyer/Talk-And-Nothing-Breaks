public static class Levels {

    public static string GetGenerator(string name) {
        if (name == "first") {
            return "{    \"name\": \"Not So Bad\",    \"description\": \"Your first dive into trap defusal! You've got plenty of time to deal with a few modules.\",    \"width\": 3,    \"height\": 2,    \"time\": 300,    \"numModules\": 2,    \"generatorType\": \"random\",    \"modules\": [        {            \"name\": \"boolean\",            \"max\": 2        },        {            \"name\": \"addition\",            \"max\": 1        }    ]}";
        } else if (name == "second") {
            return "{    \"name\": \"Enter Alchemy\",    \"description\": \"The Alchemy Module can have difficult to identify glyphs. Be prepared!\",    \"width\": 3,    \"height\": 2,    \"time\": 300,    \"numModules\": 3,    \"generatorType\": \"random\",    \"modules\": [        {            \"name\": \"boolean\",            \"min\": 1,            \"max\": 1        },        {            \"name\": \"addition\",            \"min\": 1,            \"max\": 1        }    ],    \"pools\": [        {            \"name\": \"alchemy\",            \"min\": 1,            \"max\": 1,            \"modules\": [                {                    \"name\": \"alchemy\"                }            ]        }    ]}";
        } else if (name == "cipherIntro") {
            return "{    \"name\": \"Cipher Intro\",    \"description\": \"Ciphers are everyone's favorite way to encode data! Your experts should probably have the alphabet written out for this.\",    \"width\": 3,    \"height\": 2,    \"time\": 300,    \"numModules\": 4,    \"generatorType\": \"random\",    \"modules\": [        {            \"name\": \"cipher\",            \"min\": 1,            \"max\": 1        }    ],    \"pools\": [        {            \"name\": \"easy-modules\",            \"min\": 3,            \"max\": 3,            \"modules\": [                {                    \"name\": \"boolean\"                },                {                    \"name\": \"addition\"                }            ]        }    ]}";
        } else if (name == "brightIntro") {
            return "{    \"name\": \"Bright Intro\",    \"description\": \"3 lights. 1 'simple' diagram. 1 mean module.\",    \"width\": 3,    \"height\": 2,    \"time\": 180,    \"numModules\": 1,    \"generatorType\": \"random\",    \"modules\": [        {            \"name\": \"bright\"        }    ]}";
        } else if (name == "warmingUp") {
            return "{    \"name\": \"Warming Up\",    \"description\": \"You should be well acquainted with the 5 different module types.\",    \"width\": 3,    \"height\": 2,    \"time\": 300,    \"numModules\": 4,    \"generatorType\": \"random\",    \"pools\": [        {            \"name\": \"easy\",            \"weight\": \"5\",            \"max\": 3,            \"min\": 1,            \"modules\": [                {                    \"name\": \"boolean\",                    \"weight\": 5,                    \"max\": 2                },                {                    \"name\": \"addition\",                    \"weight\": 5,                    \"max\": 2                },                {                    \"name\": \"alchemy\",                    \"weight\": 1,                    \"max\": 1                }            ]        },        {            \"name\": \"hard\",            \"weight\": \"2\",            \"max\": 2,            \"min\": 1,            \"modules\": [                {                    \"name\": \"alchemy\",                    \"weight\": 4,                    \"max\": 1                },                {                    \"name\": \"bright\",                    \"weight\": 2,                    \"max\": 1                },                {                    \"name\": \"cipher\",                    \"weight\": 2,                    \"max\": 1                }            ]        }    ]}";
        } else if (name == "doubleTrouble") {
            return "{    \"name\": \"Double Trouble\",    \"description\": \"Six modules!\",    \"width\": 3,    \"height\": 2,    \"time\": 300,    \"numModules\": 6,    \"generatorType\": \"random\",    \"pools\": [        {            \"name\": \"easy\",            \"weight\": 10,            \"modules\": [                {                    \"name\": \"boolean\",                    \"max\": 2                },                {                    \"name\": \"alchemy\",                    \"max\": 2                }            ]        },        {            \"name\": \"intermediate\",            \"weight\": 5,            \"modules\": [                {                    \"name\": \"cipher\",                    \"max\": 2                },                {                    \"name\": \"addition\",                    \"max\": 2                }            ]        },        {            \"name\": \"hard\",            \"weight\": 3,            \"modules\": [                {                    \"name\": \"bright\",                    \"max\": 1                }            ]        }    ]}";
        } else if (name == "iAmSpeed") {
            return "{    \"name\": \"I Am Speed\",    \"description\": \"Speed is of the essence, you have half the normal time to defuse 4 modules.\",    \"width\": 3,    \"height\": 2,    \"time\": 150,    \"numModules\": 4,    \"generatorType\": \"random\",    \"pools\": [        {            \"name\": \"easy\",            \"weight\": 10,            \"modules\": [                {                    \"name\": \"boolean\",                    \"max\": 1                },                {                    \"name\": \"alchemy\",                    \"max\": 2                }            ]        },        {            \"name\": \"intermediate\",            \"weight\": 5,            \"modules\": [                {                    \"name\": \"cipher\",                    \"max\": 2,                    \"weight\": 5                },                {                    \"name\": \"addition\",                    \"max\": 2,                    \"weight\": 3                }            ]        }    ]}";
        } else if (name == "ritual") {
            return "{    \"name\": \"The Ritual\",    \"description\": \"I hope you like alchemy.\",    \"width\": 3,    \"height\": 2,    \"time\": 180,    \"numModules\": 5,    \"generatorType\": \"random\",    \"modules\": [        {            \"name\": \"alchemy\"        }    ]}";
        } else if (name == "theWall") {
            return "{    \"name\": \"The Wall\",    \"description\": \"Who said traps had to be 3x2?\",    \"width\": 4,    \"height\": 4,    \"time\": 600,    \"numModules\": 10,    \"generatorType\": \"random\",    \"pools\": [        {            \"name\": \"dont-spam\",            \"min\": 2,            \"max\": 3,            \"modules\": [                {                    \"name\": \"addition\"                },                {                    \"name\": \"bright\"                }            ]        },        {            \"name\": \"easy\",            \"modules\": [                {                    \"name\": \"boolean\",                    \"weight\": 10                },                {                    \"name\": \"alchemy\",                    \"weight\": 10                },                {                    \"name\": \"cipher\",                    \"weight\": 3                }            ]        }    ]}";
        } else if (name == "iAmHardcore") {
            return "{    \"name\": \"I Am Hardcore\",    \"description\": \"The ultimate test!\",    \"width\": 3,    \"height\": 2,    \"time\": 300,    \"numModules\": 6,    \"generatorType\": \"random\",    \"pools\": [        {            \"name\": \"dont-spam\",            \"modules\": [                {                    \"name\": \"addition\"                },                {                    \"name\": \"bright\"                }            ]        },        {            \"name\": \"easy\",            \"max\": 2,            \"modules\": [                {                    \"name\": \"boolean\",                    \"weight\": 7,                    \"max\": 1                },                {                    \"name\": \"alchemy\",                    \"weight\": 8,                    \"max\": 1                },                {                    \"name\": \"cipher\",                    \"weight\": 10,                    \"max\": 1                }            ]        }    ]}";
        }
        return "{    \"name\": \"Not So Bad\",    \"description\": \"Your first dive into trap defusal! You've got plenty of time to deal with a few modules.\",    \"width\": 3,    \"height\": 2,    \"time\": 300,    \"numModules\": 3,    \"generatorType\": \"random\",    \"modules\": [        {            \"name\": \"boolean\",            \"max\": 2        },        {            \"name\": \"addition\",            \"max\": 1        }    ]}";
    }

}