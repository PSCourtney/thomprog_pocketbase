/// <reference path="../pb_data/types.d.ts" />
migrate((db) => {
  const dao = new Dao(db)
  const collection = dao.findCollectionByNameOrId("ec8cpvma1lj1dx1")

  collection.updateRule = ""
  collection.deleteRule = ""

  return dao.saveCollection(collection)
}, (db) => {
  const dao = new Dao(db)
  const collection = dao.findCollectionByNameOrId("ec8cpvma1lj1dx1")

  collection.updateRule = "@request.auth.id != \"\""
  collection.deleteRule = "@request.auth.id != \"\""

  return dao.saveCollection(collection)
})
