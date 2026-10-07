#!/bin/sh

set -e

ELASTICSEARCH_URL="http://elasticsearch:9200"
INDEX_NAME="cloudpins_documents"
SYNONYMS_NAME="cloudpins-synonyms"

echo "Waiting for Elasticsearch..."

until curl -sf "$ELASTICSEARCH_URL" > /dev/null; do
  sleep 3
done

echo "Creating synonym set..."

curl -sf -X PUT "$ELASTICSEARCH_URL/_synonyms/$SYNONYMS_NAME" \
  -H "Content-Type: application/json" \
  -d '{
    "synonyms_set": [
      {
        "id": "woman-girl",
        "synonyms": "woman, girl, female"
      },
      {
        "id": "car-vehicle",
        "synonyms": "car, vehicle, automobile, automotive"
      },
      {
        "id": "motorcycle-bike",
        "synonyms": "motorcycle, bike, motorbike"
      },
      {
        "id": "gaming-games",
        "synonyms": "gaming, games, gamer"
      },
      {
        "id": "food-meal",
        "synonyms": "food, meal, dish"
      }
    ]
  }'

INDEX_EXISTS=$(curl -s -o /dev/null -w "%{http_code}" \
  "$ELASTICSEARCH_URL/$INDEX_NAME")

if [ "$INDEX_EXISTS" = "200" ]; then
  echo "Pins index already exists."
else
  echo "Creating pins index..."

  curl -sf -X PUT "$ELASTICSEARCH_URL/$INDEX_NAME" \
    -H "Content-Type: application/json" \
    -d '{
      "settings": {
        "analysis": {
          "filter": {
            "cloudpins_synonyms": {
              "type": "synonym_graph",
              "synonyms_set": "cloudpins-synonyms",
              "updateable": true
            }
          },
          "analyzer": {
            "cloudpins_index": {
              "type": "custom",
              "tokenizer": "standard",
              "filter": ["lowercase"]
            },
            "cloudpins_search": {
              "type": "custom",
              "tokenizer": "standard",
              "filter": [
                "lowercase",
                "cloudpins_synonyms"
              ]
            }
          }
        }
      },
      "mappings": {
        "properties": {
          "id": {
            "type": "keyword"
          },
          "ownerId": {
            "type": "keyword"
          },
          "boardId": {
            "type": "keyword"
          },
          "imageUrl": {
            "type": "keyword"
          },
          "thumbnailUrl": {
            "type": "keyword"
          },
          "title": {
            "type": "text",
            "analyzer": "cloudpins_index",
            "search_analyzer": "cloudpins_search"
          },
          "description": {
            "type": "text",
            "analyzer": "cloudpins_index",
            "search_analyzer": "cloudpins_search"
          },
          "tags": {
            "type": "text",
            "analyzer": "cloudpins_index",
            "search_analyzer": "cloudpins_search"
          },
          "likesCount": {
            "type": "integer"
          },
          "createdAt": {
            "type": "date"
          }
        }
      }
    }'
fi

SUGGESTIONS_INDEX="cloudpins_suggestions"

SUGGESTIONS_EXISTS=$(curl -s -o /dev/null -w "%{http_code}" \
  "$ELASTICSEARCH_URL/$SUGGESTIONS_INDEX")

if [ "$SUGGESTIONS_EXISTS" = "200" ]; then
  echo "Suggestions index already exists."
else
  echo "Creating suggestions index..."

  curl -sf -X PUT "$ELASTICSEARCH_URL/$SUGGESTIONS_INDEX" \
    -H "Content-Type: application/json" \
    -d '{
      "mappings": {
        "properties": {
          "id": {
            "type": "keyword"
          },
          "text": {
            "type": "text"
          },
          "normalizedText": {
            "type": "keyword"
          },
          "frequency": {
            "type": "integer"
          },
          "source": {
            "type": "keyword"
          },
          "lastSeenAt": {
            "type": "date"
          }
        }
      }
    }'
fi

echo "Elasticsearch initialization completed."
