window.addEventListener('scroll', function () {
  var div = document.getElementById('header');

  // Obtem a distância da div ao topo da página
  var distance = div.offsetTop;

  // Obtem a posição de rolagem da página
  var scrollTop = document.documentElement.scrollTop;

  if (scrollTop > distance) {
    // Adiciona a classe 'fixed' quando a página for rolada para baixo além da posição da div
    div.classList.add('fixed');
  } else {
    // Remove a classe 'fixed' quando a página for rolada para cima e a div voltar à sua posição original
    div.classList.remove('fixed');
  }
});
