#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <semaphore.h>
#include <stdbool.h>

struct no {
    int valor;
    struct no* proximo;
};

struct lista {
    int quantidade;
    struct no* primeiro;
    struct no* ultimo;
};

typedef struct no No;
typedef struct lista Lista;

No* iniciaNo(int valor) {
    No* no = (No*)malloc(sizeof(No));
    no->valor = valor;
    no->proximo = NULL;

    return no;
}

void liberaNo(No* no) {
    free(no);
}

Lista* iniciaLista() {
    Lista* lista = (Lista*)malloc(sizeof(Lista));
    lista->quantidade = 0;
    lista->primeiro = NULL;
    lista->ultimo = NULL;
}

void liberaLista(Lista* lista) {
    No* noAux = lista->primeiro;

    while (noAux != NULL) {
        No* noAux2 = noAux->proximo;
        liberaNo(noAux);

        noAux = noAux2;
    }

    free(lista);
}

void addNo(Lista* lista, No* no) {
    if (lista->quantidade == 0) {
        lista->primeiro = no;
        lista->ultimo = no;
        lista->quantidade = 1;

        return;
    }

    No* penultimo = lista->ultimo;
    penultimo->proximo = no;
    lista->ultimo = no;
    lista->quantidade++;
}
/*
true - função removeu com sucesso
false - erro (tentando acessar no inexistente)
*/
bool removeNo(Lista* lista, int indice) {
    if (lista->quantidade < indice + 1) {
        return false;
    }
    //Remover primeiro e último
    if (indice == 0 && lista->quantidade == 1) {
        No* removido = lista->primeiro;
        lista->primeiro == NULL;
        lista->ultimo == NULL;
        lista->quantidade--;
        liberaNo(removido);
        return true;
    }
    //Remover o primeiro
    if (indice == 0) {
        No* removido = lista->primeiro;
        lista->primeiro = removido->proximo;
        lista->quantidade--;
        liberaNo(removido);
        return true;
    }

    No* anterior = lista->primeiro;
    for (int aux = 1; aux < indice; aux++) {
        anterior = anterior->proximo;
    }
    //Remover último
    if (lista->quantidade == indice + 1) {
        No* removido = anterior->proximo;
        anterior->proximo == NULL;
        lista->ultimo == anterior;
        lista->quantidade--;
        liberaNo(removido);
        return true;
    }
    No* removido = anterior->proximo;
    anterior->proximo = removido->proximo;
    lista->quantidade--;
    liberaNo(removido);
    return true;
}

