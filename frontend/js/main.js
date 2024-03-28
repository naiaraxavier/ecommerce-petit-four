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
