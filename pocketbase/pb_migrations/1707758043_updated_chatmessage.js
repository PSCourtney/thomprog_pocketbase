/// <reference path="../pb_data/types.d.ts" />
migrate((db) => {
  const dao = new Dao(db)
  const collection = dao.findCollectionByNameOrId("ec8cpvma1lj1dx1")

  collection.listRule = "@request.auth.verified = true"

  return dao.saveCollection(collection)
}, (db) => {
  const dao = new Dao(db)
  const collection = dao.findCollectionByNameOrId("ec8cpvma1lj1dx1")

  collection.listRule = "@request.auth.id != \"\""

  return dao.saveCollection(collection)
})
