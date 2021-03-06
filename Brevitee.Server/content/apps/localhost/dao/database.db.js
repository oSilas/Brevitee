var database = {
    nameSpace: "Brevitee.Application.Data",
    schemaName: "DaoTestData",
	xrefs: [
		["LeftOfManyItem", "RightOfManyItem"]
	],
    tables: [
        {
            name: "DaoBaseItem",
            cols: [
                { Name: "String", Null: false },
                { Created: "DateTime" },
                { IsCool: "Boolean" },
                { IntValue: "Int" },
                { LongValue: "Long" },
                { DecimalValue: "Decimal" },
                { ByteArrayValue: "Byte" }
            ]            
        },
        {
            name: "DaoSubItem",
            fks: [
                { DaoBaseItemId: "DaoBaseItem" }
            ],
            cols:[
                { Name: "String", Null: false },
                { Created: "DateTime" }
            ]
        },
		{
			name: "LeftOfManyItem",
			cols:[
				{ Name: "String", Null: false }
			]
		},
		{
			name: "RightOfManyItem",
			fks: [],
			cols:[
				{ Name: "String", Null: false }
			]
		}
    ]
};

//Boolean,
//Int,
//Long,
//Decimal,
//String,
//Byte,
//DateTime