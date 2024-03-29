// JAVASCRIPT PURO - DOM

// Fixa a div que contém login e carrinho no top da página
pinDivToTop = (divId) => {
  const div = document.getElementById(divId);
  // distância da div ao topo da página
  const distance = div.offsetTop;
  // posição de rolagem da página
  const scrollTop = document.documentElement.scrollTop;
  if (scrollTop > distance) {
    // Adiciona a classe 'fixed'
    div.classList.add('fixed');
  } else {
    // Remove a classe 'fixed'
    div.classList.remove('fixed');
  }
};

// Faz botão voltar ao topo aparecer na tela
scrollTop = () => {
  const btn = document.getElementById('scroll-top');
  const scrollTop = document.documentElement.scrollTop;
  console.log(scrollTop)
  if (scrollTop > 500) {
    // Adiciona a classe 'active'
    btn.classList.add('active');
  } else {
    // Remove a classe 'active'
    btn.classList.remove('active');
  }
};

window.addEventListener('scroll', () => {
  pinDivToTop("header");
  scrollTop();
});

// Função genérica para abrir um elemento
openElement = (elementId) => {
  if (elementId !== 'shopping-cart') {
    document.getElementById('bkg-faded').classList.add('bkg-faded');
  }
  document.getElementById(elementId).style.display = 'block';
}

// Função genérica para fechar um elemento
closeElement = (elementId) => {
  if (elementId !== 'shopping-cart') {
    document.getElementById('bkg-faded').classList.remove('bkg-faded');
  }
  document.getElementById(elementId).style.display = 'none';
}

// Funções para manipular o carrinho de compras
document.getElementById('cartBtn').addEventListener('click', () => {
  openElement('shopping-cart');
});

document.querySelector('.close').addEventListener('click', () => {
  closeElement('shopping-cart');
});

// Funções para manipular o formulário de Nova Forma de Pagamento
document.getElementById('add-payment').addEventListener('click', () => {
  openElement('payment-method-form');
});

document.querySelector('.close-form-pay').addEventListener('click', () => {
  closeElement('payment-method-form');
});

// Funções para manipular o formulário de Novo Endereço de Entrega
document.getElementById('add-address').addEventListener('click', () => {
  openElement('new-address-form');
});

document.querySelector('.close-form-address').addEventListener('click', () => {
  closeElement('new-address-form');
});

